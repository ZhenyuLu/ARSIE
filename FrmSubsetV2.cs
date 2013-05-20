using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ARSIE.DataIO;
using ARSIE.CART;

namespace ARSIE.SupervisedClassifier
{
    public partial class FrmSubsetV2 : Form
    {
        public DataTable __refTable;
        public IList<int> __selectedIndexes;

        public delegate void OnParameterChangeEvent(object sender, SubsetArgs pe);
        public OnParameterChangeEvent ParameterChange;
        public void onParameterChange(SubsetArgs pe)
        {
            if (ParameterChange != null)
                ParameterChange(this, pe);
        }

        /// <summary>
        /// AINetParaArgs, which pass through Operation parameters
        /// </summary>
        public class SubsetArgs : EventArgs
        {
            private int __subsetTimes;
            private int __subsetLowerBound;
            private int __subsetUpperBound;
            private int[] __subsetVariables;
            public SubsetArgs(int subsetTimes, int LowerBound, int UpperBound)
            {
                __subsetTimes = subsetTimes;
                __subsetLowerBound = LowerBound;
                __subsetUpperBound = UpperBound;
            }

            public SubsetArgs(IList<int> AttributeIndexes)
            {
                __subsetVariables = new int[AttributeIndexes.Count];
                for (int i = 0; i < AttributeIndexes.Count; i++)
                    __subsetVariables[i] = AttributeIndexes[i];
            }
            public int SubsetTimes
            {
                get { return __subsetTimes; }
                set { __subsetTimes = value; }
            }
            public int SubsetLowerBound
            {
                get { return __subsetLowerBound; }
                set { __subsetLowerBound = value; }
            }
            public int SubsetUpperBound
            {
                get { return __subsetUpperBound; }
                set { __subsetUpperBound = value; }
            }
            public int[] SubsetVariables
            {
                get { return __subsetVariables; }
                set { __subsetVariables = value; }
            }
        }


        /// <summary>
        /// Default Constructor
        /// </summary>
        public FrmSubsetV2()
        {
            InitializeComponent();
            rbtnEntropy.Checked = true;
        }

        /// <summary>
        /// Overloading Constructor
        /// </summary>
        /// <param name="dt">the input datatable</param>
        public FrmSubsetV2(DataTable dt)
        {
            InitializeComponent();
            if (dt != null)
            {
                __refTable = dt;
                dgvSetup();
            }
            rbtnEntropy.Checked = true;
        }

        private void dgvSetup()
        {
            int loop = 0;

            foreach (DataColumn dc in __refTable.Columns)
            {
                if (loop <= __refTable.Columns.Count - 2)
                {
                    dgvFeatures.Rows.Add();
                    dgvFeatures.Rows[loop].Cells[0].Value = true;
                    dgvFeatures.Rows[loop].Cells[1].Value = dc.ColumnName;
                    dgvFeatures.Rows[loop].Cells[2].Value = dc.DataType.ToString();
                    dgvFeatures.Rows[loop].Cells[3].Value = "Continuous";
                }
                loop++;
            }
        }

        private void btnIdentify_Click(object sender, EventArgs e)
        {
            dgvResult.DataSource = null;

            Dictionary<string, string> indexDict = getIndexPool();
            double[] gains = new double[indexDict.Count];
            DataTable resultDt = new DataTable();
            resultDt.Columns.Add(new DataColumn("Name", typeof(string)));
            resultDt.Columns.Add(new DataColumn("InformationValue", typeof(string)));
            
            Preprocessor prep = new Preprocessor();
            Information info = new Information();
            int loop = 0;
            foreach (KeyValuePair<string, string> pair in indexDict)
            {
                int colIndex = getColumnIndexFromName(__refTable, pair.Key);
                int[] intClasses = prep.getClassFieldAsArray(__refTable, __refTable.Columns.Count - 1);
                byte[] classes = intClasses.Select(x => (byte)x).ToArray();
                if (pair.Value == "Continuous")
                {
                    double[] attributes = prep.getContinousValuesFromDataTable(__refTable, colIndex);
                    double tempOut = 0.0;
                    if (rbtnEntropy.Checked)
                    {
                        gains[loop] = info.CalcInformationGain(attributes, classes, true, out tempOut);
                    }
                    else if (rbtnGainRatio.Checked)
                    {
                        gains[loop] = info.CalcGainRatio(attributes, classes, true, out tempOut);
                    }
                    else if (rbtnGINI.Checked)
                    {
                        gains[loop] = info.CalcGiniIndex(attributes, classes, true, out tempOut);
                    }
                }
                else
                {
                    string[] attributes = prep.getDiscreteValuesFromDataTable(__refTable, colIndex);
                    if(rbtnEntropy.Checked)
                    {
                        gains[loop] = info.CalcInformationGain(attributes, classes);
                    }
                    else if(rbtnGainRatio.Checked)
                    {
                        gains[loop] = info.CalcGainRatio(attributes, classes);
                    }
                }
                DataRow dr = resultDt.NewRow();
                dr[0] = pair.Key;
                dr[1] = gains[loop].ToString("F3");
                resultDt.Rows.Add(dr);
                loop++;
            }
            updateRank(gains, int.Parse(dUDBestFields.Text), ref resultDt);
            dgvResult.DataSource = resultDt;
        }

        private void updateRank(double[] gains, int number, ref DataTable dt)
        {
            Dictionary<int, int> rankResult = rankIndexes(gains, number);
            int[] rankStr = new int[rankResult.Count];
            for(int i = 0; i< rankResult.Count ;i++)
            {
                rankStr[i] = rankResult[i]+1;
            }
            Preprocessor prep = new Preprocessor();
            prep.AppendColumn<int>(dt, "Rank", rankStr);
        }

        /// <summary>
        /// private function of rank the indexes of an array
        /// </summary>
        /// <param name="gains">an array with gain values</param>
        /// <returns>a dictionary keyed by the index of column and valued by it's related rank</returns>
        private Dictionary<int, int> rankIndexes(double[] gains, int number)
        {
            Dictionary<int, int> rankDict = new Dictionary<int, int>();
            int[] Indexes = new int[gains.Length];
            for (int i = 0; i < gains.Length; i++)
                Indexes[i] = i;
            for (int i = 0; i < gains.Length; i++)
            {
                for (int j = i; j < gains.Length; j++)
                {
                    if (gains[i] < gains[j])
                    {
                        double tgain = gains[i];
                        gains[i] = gains[j];
                        gains[j] = tgain;

                        int tIndex = Indexes[i];
                        Indexes[i] = Indexes[j];
                        Indexes[j] = tIndex;
                    }
                }
            }

            this.__selectedIndexes = new List<int>();
            for (int i = 0; i < Indexes.Length; i++)
            {
                rankDict.Add(Indexes[i], i);
                if (i < number)
                    this.__selectedIndexes.Add(Indexes[i]);
            }
            return rankDict;
        }

        /// <summary>
        /// Private function of getting the index of the column according to the name of the field
        /// </summary>
        /// <param name="dt">an instance of datatable</param>
        /// <param name="fieldName">name of the field</param>
        /// <returns>the index of the field</returns>
        private int getColumnIndexFromName(DataTable dt, string fieldName)
        {
            int index = -1;
            int loop = 0;
            foreach (DataColumn dc in dt.Columns)
            {
                if (dc.ColumnName == fieldName)
                {
                    index = loop;
                    return index;
                }
                loop++;
            }
            return index;
        }

        /// <summary>
        /// private function of getIndexPool
        /// </summary>
        /// <returns>a dictionary keyed by the field name and valued by the field type (either continuous or 
        /// discrete.</returns>
        private Dictionary<string, string> getIndexPool()
        {
            Dictionary<string, string> indexDict = new Dictionary<string,string>();
            for (int i = 0; i < dgvFeatures.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dgvFeatures.Rows[i].Cells[0].Value))
                {
                    indexDict.Add(dgvFeatures.Rows[i].Cells[1].Value.ToString(), 
                        dgvFeatures.Rows[i].Cells[3].Value.ToString());
                }
            }
            return indexDict;
        }

        private void btnBiIdentify_Click(object sender, EventArgs e)
        {
            Preprocessor prep = new Preprocessor();
            int[] classes = prep.getClassFieldAsArray(__refTable, __refTable.Columns.Count - 1);
            IList<int> uniqClassNumbers = prep.GetClasses(__refTable, __refTable.Columns.Count - 1);
            Dictionary<string, string> indexDict = getIndexPool();
            double[,] gains = new double[uniqClassNumbers.Count, indexDict.Count];
            int index = 0;

            Information info = new Information();
            foreach (int classNumber in uniqClassNumbers)
            {
                int loop = 0;
                foreach (KeyValuePair<string, string> pair in indexDict)
                {
                     int colIndex = getColumnIndexFromName(__refTable, pair.Key);
                    int[] binClasses = new int[classes.Length];
                    for (int j = 0; j < binClasses.Length; j++)
                    {
                        if (classes[j] == classNumber)
                            binClasses[j] = 1;
                        else
                            binClasses[j] = 0;
                    }
                    byte[] tmpClasses = binClasses.Select(x => (byte)x).ToArray();
                    if (pair.Value == "Continuous")
                    {
                        double[] attributes = prep.getContinousValuesFromDataTable(__refTable, colIndex);
                        double tempOut = 0.0;
                        
                        if(rbtnEntropy.Checked)
                            gains[index, loop] = info.CalcInformationGain(attributes, tmpClasses, true, out tempOut);
                        else if(rbtnGainRatio.Checked)
                            gains[index, loop] = info.CalcGainRatio(attributes, tmpClasses, true, out tempOut);
                    }
                    else
                    {
                        string[] attributes = prep.getDiscreteValuesFromDataTable(__refTable, colIndex);
                        if (rbtnEntropy.Checked)
                            gains[index, loop] = info.CalcInformationGain(attributes, tmpClasses);
                        else if (rbtnGainRatio.Checked)
                            gains[index, loop] = info.CalcGainRatio(attributes, tmpClasses);
                    }
                    loop++;
                }
                index++;
            }
            updateDGVBinResult(gains, uniqClassNumbers, indexDict);
        }

        /// <summary>
        /// Update the result in the datagridview according to the binary identification result
        /// </summary>
        /// <param name="binResult">a matrix where each row represents a class and each column represents a feature</param>
        private void updateDGVBinResult(double[,] binResult, IList<int> uniqClasses, Dictionary<string, string> indexDict)
        {
            dgvResult.DataSource = null;

            DataTable tmpDt = new DataTable();
            tmpDt.Columns.Add(new DataColumn("FeatureName", typeof(string)));
            for (int i = 0; i < uniqClasses.Count; i++)
            {
                tmpDt.Columns.Add(new DataColumn("Class" + uniqClasses[i].ToString(), typeof(double)));
            }

            int l = 0;
            foreach (KeyValuePair<string, string> pair in indexDict)
            {
                DataRow dr = tmpDt.NewRow();
                dr[0] = pair.Key;
                for (int j = 0; j <= binResult.GetUpperBound(0); j++)
                {
                    dr[j + 1] = binResult[j, l];
                }
                tmpDt.Rows.Add(dr);
                l++;
            }
            dgvResult.DataSource = tmpDt;
        }

    }
}
