using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WPattern.Pattern.Recognition.Core;
using WPattern.Pattern.Recognition.Core.Algorithms;
using WPattern.Pattern.Recognition.Core.Algorithms.Enums;
using WPattern.Pattern.Recognition.Core.Algorithms.Interfaces;
using WPattern.Pattern.Recognition.Core.Algorithms.Utils;
using WPattern.Pattern.Recognition.Core.Beans;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Enums;
using WPattern.Pattern.Recognition.Core.GeneticAlgorithm.Interfaces;
using WPattern.Pattern.Recognition.Core.Interfaces;
using WPattern.Pattern.Recognition.Interfaces;
using Binding = System.Windows.Data.Binding;
using MessageBox = System.Windows.MessageBox;

namespace WPattern.Pattern.Recognition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, ILogger
    {
        #region Properties
        private ILoader Loader { get; set; }

        private SampleBean Sample { get; set; }

        private IGeneticAlgorithm GeneticAlgorithm { get; set; }

        private ClassifierType ClassifierType { get; set; }

        private NormalizationType NormalizationType { get; set; }

        private CrossValidationType CrossValidationType { get; set; }

        private CrossoverType CrossoverType { get; set; }

        private MutationType MutationType { get; set; }

        private NaturalSelectionType NaturalSelectionType { get; set; }
        #endregion

        #region Constructors
        public MainWindow()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }
        #endregion

        #region Public Methods (ILogger)
        public void LogMessage(string message)
        {
            txtLogger.AppendText(message + "\n");
        }

        public void ClearLog()
        {
            txtLogger.Clear();
        }
        #endregion

        #region Events
        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            LoadFile(Directory.GetCurrentDirectory() + "\\..\\..\\..\\Solution Items\\iris_samples.txt");
        }

        private void btnLoadSamples_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.RestoreDirectory = true;
            openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    LoadFile(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }
        }

        private void btnStartGeneticAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            GeneticAlgorithmParametersBean geneticAlgorithmParameters = new GeneticAlgorithmParametersBean()
                {
                    ClassifierParametersBean = new ClassifierParametersBean()
                        {
                            K = Int32.Parse(txtK.Text),
                            NormalizationType = NormalizationType,
                            CrossValidationType = CrossValidationType,
                            ClassifierType = ClassifierType.KNN
                        },
                    CrossoverRate = Double.Parse(txtCrossover.Text),
                    MutationRate = Double.Parse(txtMutation.Text),
                    Generations = Int32.Parse(txtGenerations.Text),
                    PopulationSize = Int32.Parse(txtPopulationSize.Text),
                    CrossoverType = CrossoverType,
                    MutationType = MutationType,
                    NaturalSelection = NaturalSelectionType
                };

            LogMessage("======================================================================================================");
            LogMessage("GENETIC ALGORITHM CLASSIFICATION");
            LogMessage("Algorithm: " + ClassifierType);
            LogMessage("K: " + geneticAlgorithmParameters.ClassifierParametersBean.K);
            LogMessage("NormalizationType: " + geneticAlgorithmParameters.ClassifierParametersBean.NormalizationType);
            LogMessage("Cross Validation: LEAVE ONE OUT");
            LogMessage("Population Size: " + geneticAlgorithmParameters.PopulationSize);
            LogMessage("Generations: " + geneticAlgorithmParameters.Generations);
            LogMessage("Crossover Rate (%): " + geneticAlgorithmParameters.CrossoverRate);
            LogMessage("Crossover Type: " + geneticAlgorithmParameters.CrossoverType);
            LogMessage("Mutation Rate (%): " + geneticAlgorithmParameters.MutationRate);
            LogMessage("Mutation Type: " + geneticAlgorithmParameters.MutationType);

            IndividualBean bestSolution = GeneticAlgorithm.Execute(Sample, geneticAlgorithmParameters);

            LogMessage("\nSOLUTION FOUNDED");

            for (int i = 0; i < bestSolution.ValidAttributes.Length; i++)
            {
                if (bestSolution.ValidAttributes[i])
                {
                    LogMessage((i + 1) + " - " + Sample.Structure.AttributeNames[i]);
                }
            }

            String message = "\nCorrect Classification (%) = " + bestSolution.ClassifierResult.CorrectClassifications;
            message += "\nWrong Classification (%) = " + bestSolution.ClassifierResult.WrongClassifications;
            message += "\n\nConfusion Matrix\n";
            message += bestSolution.ClassifierResult.ConfusionMatrix.ConfusionMatrix.Aggregate("", (current1, t) => t.Aggregate(current1, (current, t1) => current + (t1 + " ")) + "\n");

            LogMessage(message);

            tabLogger.Focus();
        }

        private void btnStartClassification_Click(object sender, RoutedEventArgs e)
        {
            int k = Int32.Parse(txtK.Text);

            LogMessage("======================================================================================================");
            LogMessage("CLASSIFICATION => Algorithm [" + ClassifierType + "], K [" + k + "], NormalizationType [" + NormalizationType + "], Cross Validation [" + CrossValidationType + "]");

            IClassifier classifier = ClassifierFactory.GetClassifierInstance(ClassifierType);

            ClassifierResultBean classifierResult = classifier.Classify(Sample, new ClassifierParametersBean()
            {
                K = Int32.Parse(txtK.Text),
                NormalizationType = NormalizationType,
                CrossValidationType = CrossValidationType,
                ClassifierType = ClassifierType
            });

            String message = "\nCorrect Classification (%) = " + classifierResult.CorrectClassifications;
            message += "\nWrong Classification (%) = " + classifierResult.WrongClassifications;
            message += "\n\nConfusion Matrix\n";
            message += classifierResult.ConfusionMatrix.ConfusionMatrix.Aggregate("", (current1, t) => t.Aggregate(current1, (current, t1) => current + (t1 + " ")) + "\n");

            LogMessage(message);

            tabLogger.Focus();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearLog();
        }

        private void grpCrossValidationLeaveOneOut_Checked(object sender, RoutedEventArgs e)
        {
            CrossValidationType = CrossValidationType.LEAVE_ONE_OUT;
        }

        private void grpNormalizationNone_Checked(object sender, RoutedEventArgs e)
        {
            NormalizationType = NormalizationType.NONE;
        }

        private void grpNormalizationZScore_Checked(object sender, RoutedEventArgs e)
        {
            NormalizationType = NormalizationType.Z_SCORE;
        }

        private void grpCrossoverTournament_Checked(object sender, RoutedEventArgs e)
        {
            CrossoverType = CrossoverType.TOURNAMENT;
        }

        private void grpCrossoverSimple_Checked(object sender, RoutedEventArgs e)
        {
            CrossoverType = CrossoverType.SIMPLE;
        }

        private void grpCrossoverRoulette_Checked(object sender, RoutedEventArgs e)
        {
            CrossoverType = CrossoverType.ROULETTE;
        }

        private void grpMutationSimple_Checked(object sender, RoutedEventArgs e)
        {
            MutationType = MutationType.SIMPLE;
        }

        private void grpMutationSwitch_Checked(object sender, RoutedEventArgs e)
        {
            MutationType = MutationType.SWITCH;
        }

        private void grpNaturalSelectionBests_Checked(object sender, RoutedEventArgs e)
        {
            NaturalSelectionType = NaturalSelectionType.BESTS;
        }

        private void grpAlgorithmKnn_Checked(object sender, RoutedEventArgs e)
        {
            ClassifierType = ClassifierType.KNN;
        }

        private void grpAlgorithmPrototype_Checked(object sender, RoutedEventArgs e)
        {
            ClassifierType = ClassifierType.PROTOTYPE;
        }

        private void grpAlgorithmKnnPrototype_Checked(object sender, RoutedEventArgs e)
        {
            ClassifierType = ClassifierType.KNN_PROTOTYPE;
        }
        #endregion

        #region Private Methods
        private void InitializeCustomComponents()
        {
            Loader = new Loader();
            GeneticAlgorithm = new GeneticAlgorithm();
        }

        private void LoadFile(String fileName)
        {
            try
            {
                Sample = Loader.LoadFile(fileName);

                LogMessage("File with samples loaded => " + fileName.Substring(fileName.LastIndexOf('\\') + 1));
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButton.OK);
                return;
            }

            if (Sample != null)
            {
                sampleRecords.Columns.Clear();
                sampleRecords.AutoGenerateColumns = false;

                for (int i = 0; i < Sample.Structure.AmountAttributes; i++)
                {
                    DataGridTextColumn textColumn = new DataGridTextColumn();

                    textColumn.Header = Sample.Structure.AttributeNames[i];
                    Binding attributeBinding = new Binding("Attributes[" + i + "]");
                    attributeBinding.Mode = BindingMode.OneWay;
                    textColumn.Binding = attributeBinding;

                    sampleRecords.Columns.Add(textColumn);
                }

                DataGridTextColumn classColumn = new DataGridTextColumn();

                classColumn.Header = Sample.Structure.AttributeNames.Last();
                Binding classBinding = new Binding("Class");
                classBinding.Mode = BindingMode.OneWay;
                classColumn.Binding = classBinding;

                sampleRecords.Columns.Add(classColumn);

                sampleRecords.ItemsSource = Sample.Records.Select(r => new { r.Attributes, r.Class });
            }
        }
        #endregion
    }
}
