using System;

namespace WPattern.Pattern.Recognition.Core.Beans
{
    public class ClassifierResultBean
    {
        #region Variables
        private ConfusionMatrixBean _confusionMatrix;
        #endregion

        #region Properties
        public ConfusionMatrixBean ConfusionMatrix
        {
            get { return _confusionMatrix; }
            set
            {
                _confusionMatrix = value;
                CalculatePercentageClassifications();
            }
        }

        public double CorrectClassifications { get; set; }

        public double WrongClassifications { get; set; }
        #endregion

        #region Public Methods
        public void CalculatePercentageClassifications()
        {
            int correctClassification = 0, wrongClassification = 0;

            for (int i = 0; i < ConfusionMatrix.ConfusionMatrix.Length; i++)
            {
                for (int j = 0; j < ConfusionMatrix.ConfusionMatrix[i].Length; j++)
                {
                    if (i == j)
                    {
                        correctClassification += ConfusionMatrix.ConfusionMatrix[i][j];
                    }
                    else
                    {
                        wrongClassification += ConfusionMatrix.ConfusionMatrix[i][j];
                    }
                }
            }

            CorrectClassifications = 100.0 * ((double) correctClassification/(correctClassification + wrongClassification));
            WrongClassifications = 100.0 - CorrectClassifications;
        }
        #endregion
    }
}