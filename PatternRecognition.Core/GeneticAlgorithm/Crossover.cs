using System;
using System.Linq;
using System.Collections.Generic;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces;

namespace WPattern.Pattern.Recognition.Core.GeneticAlgorithm
{
    public class Crossover : ICrossover
    {
        #region Properties
        private Random Random { get; set; }
        #endregion

        #region Constructors
        public Crossover()
        {
            Random = new Random((int)DateTime.Now.TimeOfDay.TotalMilliseconds);
        }
        #endregion

        #region Public Methods (ICrossover)
        public List<IndividualBean> ExecuteCrossover(IndividualBean individual1, IndividualBean individual2)
        {
            int amountAttributes = individual1.ValidAttributes.Length;

            IndividualBean children1 = new IndividualBean(amountAttributes);
            IndividualBean children2 = new IndividualBean(amountAttributes);

            int startIndex = Random.Next(amountAttributes);
            int lastIndex = Random.Next(amountAttributes);

            if (startIndex > lastIndex)
            {
                int aux = startIndex;
                startIndex = lastIndex;
                lastIndex = aux;
            }

            for (int i = 0; i < amountAttributes; i++)
            {
                if ((i >= startIndex) && (i <= lastIndex))
                {
                    children1.ValidAttributes[i] = individual2.ValidAttributes[i];
                    children2.ValidAttributes[i] = individual1.ValidAttributes[i];
                }
                else
                {
                    children1.ValidAttributes[i] = individual1.ValidAttributes[i];
                    children2.ValidAttributes[i] = individual2.ValidAttributes[i];
                }
            }

            List<IndividualBean> childrens = new List<IndividualBean>(2);

            childrens.Add(children1);
            childrens.Add(children2);

            return childrens;
        }

        public List<IndividualBean> SelectIndividualToCrossover(List<IndividualBean> individuals, CrossoverType crossoverType)
        {
            switch (crossoverType)
            {
                case CrossoverType.SIMPLE:
                    return SimpleSelection(individuals);

                case CrossoverType.TOURNAMENT:
                    return TournamentSelection(individuals);

                case CrossoverType.ROULETTE:
                    return RouletteSelection(individuals, 2);
            }

            return null;
        }
        #endregion

        #region Private Methods
        private List<IndividualBean> SimpleSelection(List<IndividualBean> individuals)
        {
            List<IndividualBean> seletecIndividuals = new List<IndividualBean>(2);

            seletecIndividuals.Add(individuals[Random.Next(individuals.Count)]);
            seletecIndividuals.Add(individuals[Random.Next(individuals.Count)]);

            return seletecIndividuals;
        }

        private List<IndividualBean> TournamentSelection(List<IndividualBean> individuals)
        {
            List<IndividualBean> seletecIndividuals = new List<IndividualBean>
                {
                    RouletteSelection(individuals, 3).OrderByDescending(i => i.Fitness).First(),
                    RouletteSelection(individuals, 3).OrderByDescending(i => i.Fitness).First()
                };

            return seletecIndividuals;
        }

        private List<IndividualBean> RouletteSelection(List<IndividualBean> individuals, int amountSelectedIndividuals)
        {
            List<IndividualBean> seletecIndividuals = new List<IndividualBean>(amountSelectedIndividuals);
            double fitnessTotal = individuals.Sum(i => i.Fitness);

            for (int i = 0; i < amountSelectedIndividuals; i++)
            {
                double sumFitness = 0.0;
                double randomlySelectedNumber = Random.NextDouble();
    
                foreach(IndividualBean individual in individuals)
                {
                    sumFitness += individual.Fitness;

                    if (randomlySelectedNumber <= (sumFitness / fitnessTotal))
                    {
                        seletecIndividuals.Add(individual);
                        break;
                    }
                }
            }

            return seletecIndividuals;
        }
        #endregion
    }
}