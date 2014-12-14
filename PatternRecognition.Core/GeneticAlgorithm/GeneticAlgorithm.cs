using System;
using System.Linq;
using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.Algorithms;
using WPattern.Pattern.Recognition.Core.Algorithms.Enums;
using WPattern.Pattern.Recognition.Core.Algorithms.Interfaces;
using WPattern.Pattern.Recognition.Core.Algorithms.Utils;
using WPattern.Pattern.Recognition.Core.Beans;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm
{
    public class GeneticAlgorithm : IGeneticAlgorithm
    {
        #region Properties
        private Dictionary<String, IndividualBean> FitnessByIndividual { get; set; }

        private ICrossover Crossover { get; set; }

        private IMutation Mutation { get; set; }

        private Random Random { get; set; }

        private INaturalSelection NaturalSelection { get; set; }
        #endregion

        #region Constructors
        public GeneticAlgorithm()
        {
            Random = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
            FitnessByIndividual = new Dictionary<string, IndividualBean>();
            Crossover = new Crossover();
            Mutation = new Mutation();
            NaturalSelection = new NaturalSelection();
        }
        #endregion

        #region Public Methods (IGeneticAlgorithm)
        public IndividualBean Execute(SampleBean sample, GeneticAlgorithmParametersBean parameters)
        {
            int countGenerations = 0;

            // 1. Create population.
            List<IndividualBean> population = CreatePopulation(parameters.PopulationSize, sample.Structure.AmountAttributes);
            List<IndividualBean> chieldrens;

            population = EvaluateIndividuals(sample, parameters, population).OrderByDescending(p => p.Fitness).ToList();

            // 2. Validate the best solution.
            while ((countGenerations++ < parameters.Generations) && (population.First().Fitness < 1.0))
            {
                // 3. Selection and Crossover.
                int amountSelectedIndividuals = (int) Math.Round((parameters.CrossoverRate * parameters.PopulationSize) / 200.0);
                chieldrens = new List<IndividualBean>();

                for (int i = 0; i < amountSelectedIndividuals; i++)
                {
                    List<IndividualBean> crossoverIndividuals = Crossover.SelectIndividualToCrossover(population, parameters.CrossoverType);
                    chieldrens.AddRange(Crossover.ExecuteCrossover(crossoverIndividuals[0], crossoverIndividuals[1]));
                }

                // 4. Mutation.
                chieldrens = Mutation.ExecuteMutation(chieldrens, parameters.MutationRate, parameters.MutationType);

                // 5. Natural selection.
                chieldrens = EvaluateIndividuals(sample, parameters, chieldrens);
                population = NaturalSelection.ExecuteNaturalSelection(population, chieldrens, parameters.PopulationSize, parameters.NaturalSelection);
            
                System.Diagnostics.Debug.WriteLine("=> Generetion " + countGenerations + " with the best fitness " + population.First().Fitness);
            }

            return population.First();
        }
        #endregion

        #region Private Methods
        private List<IndividualBean> CreatePopulation(int populationSize, int amountAttributes)
        {
            List<IndividualBean> population = new List<IndividualBean>(populationSize);

            for (int i = 0; i < populationSize; i++)
            {
                IndividualBean randomIndividual = new IndividualBean(amountAttributes);

                for (int j = 0; j < randomIndividual.ValidAttributes.Length; j++)
                {
                    randomIndividual.ValidAttributes[j] = (Random.Next() % 2) == 1;
                }

                if (!population.Contains(randomIndividual))
                {
                    population.Add(randomIndividual);
                }
            }

            return population;
        }

        private List<IndividualBean> EvaluateIndividuals(SampleBean sample, GeneticAlgorithmParametersBean parameters, List<IndividualBean> individuals)
        {
            foreach (IndividualBean individual in individuals)
            {
                string fitnessKey = individual.GetKey();
                IndividualBean fitnessValue;
                SampleBean sampleByIndividual = SampleByIndividual(sample, individual);

                if (!FitnessByIndividual.TryGetValue(fitnessKey, out fitnessValue))
                {
                    fitnessValue = new IndividualBean(sample.Structure.AmountAttributes);

                    if (sampleByIndividual.Structure.AmountAttributes > 0)
                    {
                        IClassifier classifier = ClassifierFactory.GetClassifierInstance(parameters.ClassifierParametersBean.ClassifierType);
                        ClassifierResultBean classifierResult = classifier.Classify(sampleByIndividual, parameters.ClassifierParametersBean);
                        fitnessValue.Fitness = classifierResult.CorrectClassifications / 100.0;
                        fitnessValue.ClassifierResult = classifierResult;
                    }
                    else
                    {
                        fitnessValue.Fitness = 0.0;
                    }

                    FitnessByIndividual.Add(fitnessKey, fitnessValue);
                }

                individual.Fitness = fitnessValue.Fitness;
                individual.ClassifierResult = fitnessValue.ClassifierResult;
            }

            return individuals;
        }

        private SampleBean SampleByIndividual(SampleBean sample, IndividualBean individual)
        {
            List<String> attributeNames = new List<string>();

            for (int i = 0; i < sample.Structure.AmountAttributes; i++) 
            {
                if (individual.ValidAttributes[i])
                {
                    attributeNames.Add(sample.Structure.AttributeNames[i]);
                }
            }

            attributeNames.Add(sample.Structure.AttributeNames.Last());

            SampleBean sampleByIndividual = new SampleBean(new StructureBean(attributeNames.ToArray(), sample.Structure.AmountClass));

            foreach (RecordBean record in sample.Records)
            {
                RecordBean newRecord = new RecordBean(sampleByIndividual.Structure.AmountAttributes)
                    {
                        Class = record.Class
                    };

                int countAttribute = 0;

                for (int i = 0; i < sample.Structure.AmountAttributes; i++)
                {
                    if (individual.ValidAttributes[i])
                    {
                        newRecord.Attributes[countAttribute++] = record.Attributes[i];
                    }
                }

                sampleByIndividual.Records.Add(newRecord);
            }

            return sampleByIndividual;
        }
        #endregion
    }
}