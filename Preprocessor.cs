using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;

namespace ARSIE.DataIO
{
    public class Preprocessor
    {
        /// <summary>
        /// Constructor of data
        /// </summary>
        public Preprocessor() { }
        /// <summary>
        /// Function of TablePreprocessing
        /// </summary>
        /// <returns>the table that has been processed and organized in certain format and easily be processed</returns>
        public DataTable ExtractColumns(DataTable referenceTable, IList<int> attributeIndexes)
        {
            DataTable result = new DataTable();
            //Create the table's framework
            foreach (int id in attributeIndexes)
            {
                DataColumn dc = new DataColumn();
                dc.Caption = referenceTable.Columns[id].ColumnName;
                dc.ColumnName = referenceTable.Columns[id].ColumnName;
                result.Columns.Add(dc);
            }
            for (int i = 0; i < referenceTable.Rows.Count; i++)
            {
                DataRow dr = result.NewRow();
                for (int j = 0; j < attributeIndexes.Count; j++)
                {
                    dr[j] = referenceTable.Rows[i][attributeIndexes[j]];
                }
                result.Rows.Add(dr);
            }
            return result;
        }
        /// <summary>
        /// Function of TablePreprocessing
        /// </summary>
        /// <returns>the table that has been processed and organized in certain format and easily be processed</returns>
        public DataTable ExtractReference(DataTable referenceTable, IList<int> attributeIndexes)
        {
            DataTable result = new DataTable();
            //Create the table's framework
            foreach (int id in attributeIndexes)
            {
                DataColumn dc = new DataColumn();
                dc.Caption = referenceTable.Columns[id].Caption;
                result.Columns.Add(dc);
            }
            for (int i = 0; i < referenceTable.Rows.Count; i++)
            {
                DataRow dr = result.NewRow();
                for (int j = 0; j < attributeIndexes.Count; j++)
                {
                    dr[j] = referenceTable.Rows[i][attributeIndexes[j]];
                }
                result.Rows.Add(dr);
            }
            return result;
        }
        /// <summary>
        /// Function of subsetDataTable, get a subset of information from a dataTable
        /// </summary>
        /// <param name="referenceTable">a datatable that will be subset</param>
        /// <param name="rowIndexes">a list of indexes indicating which rows are going to be extracted</param>
        /// <param name="columnIndexes">a list of indexes indicating which columns are going to be extracted</param>
        /// <returns>a new datatable which contain a subset of records</returns>
        public DataTable subsetDataTable(DataTable referenceTable, IList<int> rowIndexes, IList<int> columnIndexes)
        {
            DataTable result = new DataTable();
            //Create the table's framework
            foreach (int id in columnIndexes)
            {
                DataColumn dc = new DataColumn();
                dc.Caption = referenceTable.Columns[id].Caption;
                result.Columns.Add(dc);
            }
            for (int i = 0; i < referenceTable.Rows.Count; i++)
            {
                bool addRow = false;
                foreach (int value in rowIndexes)
                {
                    if (i == value)
                    {
                        addRow = true;
                        break;
                    }
                }
                if (addRow)
                {
                    DataRow dr = result.NewRow();
                    for (int j = 0; j < columnIndexes.Count; j++)
                    {
                        dr[j] = referenceTable.Rows[i][columnIndexes[j]];
                    }
                    result.Rows.Add(dr);
                }
            }
            return result;
        }

        /// <summary>
        /// Extract records according to row indexes
        /// </summary>
        /// <param name="referenceTable">a data table to extract</param>
        /// <param name="rowIndexes">a list of row indexes</param>
        /// <returns>a subset of datatable containing only selected rows</returns>
        public DataTable ExtractRows(DataTable referenceTable, IList<int> rowIndexes)
        {
            IList<int> columnIndexes = new List<int>();
            for (int i = 0; i < referenceTable.Columns.Count; i++)
                columnIndexes.Add(i);
            return subsetDataTable(referenceTable, rowIndexes, columnIndexes);
        }

        /// <summary>
        /// Create a datatable from a matrix
        /// </summary>
        /// <typeparam name="T">different data type</typeparam>
        /// <param name="matrix">a matrix containing the data to fill into a data table</param>
        /// <returns>a data table</returns>
        /// <remarks>
        /// the matrix needs to be reversed because the data read from the image is reversed, row indicate the number of attribute and
        /// column indicate the number of rows.
        /// </remarks>
        public DataTable CreateTableFromMatrixReverse<T>(T[,] matrix)
        {
            DataTable data = new DataTable();
            int rows = matrix.GetUpperBound(1) + 1;
            int columns = matrix.GetUpperBound(0) + 1;

            for (int i = 0; i < columns; i++)
            {
                DataColumn dc = new DataColumn();
                dc.Caption = "variable" + (i + 1).ToString();
                data.Columns.Add(dc);
            }
            for (int i = 0; i < rows; i++)
            {
                DataRow dr = data.NewRow();
                for (int j = 0; j < columns; j++)
                {
                    dr[j] = matrix[j, i];
                }
                data.Rows.Add(dr);
            }
            return data;
        }

        /// <summary>
        /// Get a list of row indexes which satisfy certain conditions
        /// </summary>
        /// <typeparam name="T">could be different data type</typeparam>
        /// <param name="referenceTable">a reference data table</param>
        /// <param name="columnIndex">the value in a certain column to compare with</param>
        /// <param name="conditionalValue">a value to compare with</param>
        /// <returns>a list of indexes</returns>
        public IList<int> getConditionalRowIndexes<T>(DataTable referenceTable, int columnIndex, T conditionalValue)
        {
            IList<int> rowIndexes = new List<int>();
            int loop = 0;
            foreach (DataRow dr in referenceTable.Rows)
            {
                try
                {
                    if (Convert.ChangeType(dr[columnIndex], typeof(T)).ToString() == Convert.ChangeType(conditionalValue, typeof(T)).ToString())
                        rowIndexes.Add(loop);
                }
                catch
                {
                    throw new Exception("Unable to convert data to data type of " + typeof(T).ToString());
                }
                loop++;
            }
            return rowIndexes;
        }
        /// <summary>
        /// Get a list of row indexes which satisfy certain conditions
        /// </summary>
        /// <param name="referenceTable">a reference data table</param>
        /// <param name="columnIndex">the value in a certain column to compare with</param>
        /// <param name="conditionalValue">a conditional value</param>
        /// <param name="sign">a sign indicating the relationship (could be GT, GE, EQ, LE, LT (From IDL^_^))</param>
        /// <returns>a list of indexes</returns>
        public IList<int> getConditionalRowIndexes(DataTable referenceTable, int columnIndex, double conditionalValue, string sign)
        {
            IList<int> rowIndexes = new List<int>();
            int loop = 0;
            switch (sign)
            {
                case "GT":
                    foreach (DataRow dr in referenceTable.Rows)
                    {
                        if (Convert.ToDouble(dr[columnIndex]) > conditionalValue)
                            rowIndexes.Add(loop);
                        loop++;
                    }
                    break;
                case "GE":
                    foreach (DataRow dr in referenceTable.Rows)
                    {
                        if (Convert.ToDouble(dr[columnIndex]) >= conditionalValue)
                            rowIndexes.Add(loop);
                        loop++;
                    }
                    break;
                case "EQ":
                    foreach (DataRow dr in referenceTable.Rows)
                    {
                        if (Convert.ToDouble(dr[columnIndex]) == conditionalValue)
                            rowIndexes.Add(loop);
                        loop++;
                    }
                    break;
                case "LE":
                    foreach (DataRow dr in referenceTable.Rows)
                    {
                        if (Convert.ToDouble(dr[columnIndex]) <= conditionalValue)
                            rowIndexes.Add(loop);
                        loop++;
                    }
                    break;
                case "LT":
                    foreach (DataRow dr in referenceTable.Rows)
                    {
                        if (Convert.ToDouble(dr[columnIndex]) < conditionalValue)
                            rowIndexes.Add(loop);
                        loop++;
                    }
                    break;
                default:
                    throw new Exception("Can't compare the relationship. The sign value can only be GE, GT, EQ, LE, and LT");
            }
            return rowIndexes;
        }
        /// <summary>
        /// Extract a matrix which satisfy a certain condition
        /// </summary>
        /// <typeparam name="T">could only be data type of double, float, integer, and byte</typeparam>
        /// <param name="originalMatrix">the original matrix</param>
        /// <param name="columnIndex">the field index where the value are compared</param>
        /// <param name="conditionalValue">a scalar to compare with</param>
        /// <param name="condition">a sign indicating the relationship (could be GT, GE, EQ, LE, LT (From IDL^_^))</param>
        /// <returns>a new matrix containing only records satisfy the condition</returns>
        /// <remarks>for continuous data (may not be very efficient since the condition judgement will be carried out twice)</remarks>
        public T[,] getConditionalMatrix<T>(T[,] originalMatrix, int columnIndex, double conditionalValue, string condition, out int[] rowIndexes)
        {
            //Loop through the original Matrix to find the number of records satisfy a certain condition
            int length = 0;
            int totalRecords = originalMatrix.GetUpperBound(1) + 1;
            int totalCols = originalMatrix.GetUpperBound(0) + 1;
            for (int i = 0; i < totalRecords; i++)
            {
                switch (condition)
                {
                    case "GT":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) > conditionalValue)
                            length++;
                        break;
                    case "GE":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) >= conditionalValue)
                            length++;
                        break;
                    case "EQ":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) == conditionalValue)
                            length++;
                        break;
                    case "LE":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) <= conditionalValue)
                            length++;
                        break;
                    case "LT":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) < conditionalValue)
                            length++;
                        break;
                    default:
                        throw new Exception("Can't compare the relationship. The sign value can only be GE, GT, EQ, LE, and LT");
                }
            }
            //Fetch the values
            int loop = 0;
            T[,] newMatrix = new T[length, totalRecords];
            rowIndexes = new int[length];
            for (int i = 0; i < totalRecords; i++)
            {
                switch (condition)
                {
                    case "GT":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) > Convert.ToDouble(conditionalValue))
                        {
                            for (int j = 0; j < totalCols; j++)
                            {
                                newMatrix[loop, j] = originalMatrix[i, j];
                                rowIndexes[loop] = i;
                            }
                            loop++;
                        }
                        break;
                    case "GE":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) >= Convert.ToDouble(conditionalValue))
                        {
                            for (int j = 0; j < totalCols; j++)
                            {
                                newMatrix[loop, j] = originalMatrix[i, j];
                                rowIndexes[loop] = i;
                            }
                            loop++;
                        }
                        break;
                    case "EQ":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) == Convert.ToDouble(conditionalValue))
                        {
                            for (int j = 0; j < totalCols; j++)
                            {
                                newMatrix[loop, j] = originalMatrix[i, j];
                                rowIndexes[loop] = i;
                            }
                            loop++;
                        }
                        break;
                    case "LE":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) <= Convert.ToDouble(conditionalValue))
                        {
                            for (int j = 0; j < totalCols; j++)
                            {
                                newMatrix[loop, j] = originalMatrix[i, j];
                                rowIndexes[loop] = i;
                            }
                            loop++;
                        }
                        break;
                    case "LT":
                        if (Convert.ToDouble(originalMatrix[i, columnIndex]) < Convert.ToDouble(conditionalValue))
                        {
                            for (int j = 0; j < totalCols; j++)
                            {
                                newMatrix[loop, j] = originalMatrix[i, j];
                                rowIndexes[loop] = i;
                            }
                            loop++;
                        }
                        break;
                    default:
                        throw new Exception("Can't compare the relationship. The sign value can only be GE, GT, EQ, LE, and LT");
                }
            }
            return newMatrix;
        }
        /// <summary>
        /// Convert the data in a datatable to an array
        /// </summary>
        /// <typeparam name="T">data type of the datatable</typeparam>
        /// <param name="dt">the datatable where the data were contained</param>
        /// <returns>an array containing the data from the datatable</returns>
        public double[,] ConvertDataTable2Array(DataTable dt)
        {
            double[,] data = new double[dt.Rows.Count, dt.Columns.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    try
                    {
                        data[i, j] = Convert.ToDouble(dt.Rows[i][j]);
                    }
                    catch
                    {
                        throw new Exception("Failed to convert data to an array!");
                    }
                }
            }
            return data;
        }

        /// <summary>
        ///Append a new column with default value to the original data table (Generic) 
        /// </summary>
        /// <typeparam name="T">could be either byte, integer, double or string</typeparam>
        /// <param name="dt">the original data table</param>
        /// <param name="columnName">the name of the newly added column</param>
        /// <param name="defaultValue">the default value of the newly added column</param>
        /// <returns>a newly created datatable</returns>
        public DataTable AppendColumn<T>(DataTable dt, string columnName, T defaultValue)
        {
            DataTable newTable = dt;
            DataColumn dc = new DataColumn(columnName);
            dc.DataType = defaultValue.GetType();
            dc.DefaultValue = defaultValue;
            newTable.Columns.Add(dc);
            return newTable;
        }
        /// <summary>
        ///Append a new column with values to the original data table (Generic) 
        /// </summary>
        /// <typeparam name="T">could be either byte, integer, double or string</typeparam>
        /// <param name="dt">the original data table</param>
        /// <param name="columnName">the name of the newly added column</param>
        /// <param name="defaultValue">the default value of the newly added column</param>
        /// <returns>a newly created datatable</returns>
        public DataTable AppendColumn<T>(DataTable dt, string columnName, T[] values)
        {
            DataTable newTable = dt;
            DataColumn dc = new DataColumn(columnName);
            dc.DataType = values[0].GetType();
            dc.DefaultValue = values[0];
            newTable.Columns.Add(dc);
            for (int i = 0; i < values.Length; i++)
            {
                newTable.Rows[i][newTable.Columns.Count - 1] = values[i];
            }
            return newTable;
        }

        /// <summary>
        /// get indexes of records whose value is equal to a specific value
        /// </summary>
        /// <param name="data">a dataTable</param>
        /// <param name="index">the field to compare with</param>
        /// <param name="value">the specific value (0, 1, ...)</param>
        /// <returns>the row indexes</returns>
        public IList<int> getSpecificIndexes(DataTable data, int index, int value)
        {
            IList<int> indexes = new List<int>();
            int number = 0;
            foreach (DataRow dr in data.Rows)
            {
                if (Convert.ToInt16(dr[index]) == value)
                    indexes.Add(number);
                number++;
            }
            return indexes;
        }

        /// <summary>
        /// function of sortTable
        /// </summary>
        /// <param name="referenceTable">a DataTable which needs to be sorted</param>
        /// <param name="field">the field where the sorting will be based</param>
        /// <returns>a sorted DataTable</returns>
        public DataTable sortTable(DataTable referenceTable, int field)
        {
            DataView dv = referenceTable.DefaultView;
            try
            {
                dv.Sort = referenceTable.Columns[field].Caption + " ASC";
            }
            catch
            {
                dv.Sort = "column" + field.ToString() + " ASC";
            }
            DataTable resultTable = dv.ToTable();
            return resultTable;
        }
        /// <summary>
        /// Function of FindScale
        /// </summary>
        /// <remarks>this function assumes that the class field is the last column of the DataTable</remarks>
        public double[,] FindScale(DataTable reference)
        {
            int columnCount = reference.Columns.Count;
            int rowCount = reference.Rows.Count;
            double[,] scale = new double[columnCount - 1, 2];
            //Initialize the scale values
            for (int i = 0; i < columnCount - 1; i++)
            {
                scale[i, 0] = Convert.ToDouble(reference.Rows[0][i]);
                scale[i, 1] = Convert.ToDouble(reference.Rows[0][i]);
            }
            for (int i = 0; i < columnCount - 1; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    if (Convert.ToDouble(reference.Rows[j][i]) < scale[i, 0])
                        scale[i, 0] = Convert.ToDouble(reference.Rows[j][i]);
                    else if (Convert.ToDouble(reference.Rows[j][i]) > scale[i, 1])
                        scale[i, 1] = Convert.ToDouble(reference.Rows[j][i]);
                }
            }
            return scale;
        }
        /// <summary>
        /// Function of FindScale
        /// </summary>
        /// <param name="reference">the datatable where the scale value is going to be found</param>
        /// <param name="field">an index of field where the scales are going to be selected from</param>
        /// <remarks>this function assumes that the class field is the last column of the DataTable</remarks>
        public double[] FindScale(DataTable reference, int field)
        {
            int rowCount = reference.Rows.Count;
            double[] scale = new double[2];
            try
            {
                scale[0] = Convert.ToDouble(reference.Rows[0][field]);
                scale[1] = Convert.ToDouble(reference.Rows[0][field]);
                for (int j = 0; j < rowCount; j++)
                {
                    if (Convert.ToDouble(reference.Rows[j][field]) < scale[0])
                        scale[0] = Convert.ToDouble(reference.Rows[j][field]);
                    else if (Convert.ToDouble(reference.Rows[j][field]) > scale[1])
                        scale[1] = Convert.ToDouble(reference.Rows[j][field]);
                }
                return scale;
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to find the scale for field " + field.ToString() + " because " + ex.Message);
            }
        }
        /// <summary>
        /// Function of Normalize
        /// </summary>
        /// <remarks>this function will normalize the data within the range of 0-1</remarks>
        public void Normailize(ref DataTable reference, double[,] scale)
        {
            int attributesCount = scale.GetUpperBound(0) + 1;
            //FindScale();
            for (int i = 0; i < reference.Rows.Count; i++)
            {
                for (int j = 0; j < reference.Columns.Count; j++)
                {
                    //Keep the values class number column
                    if (j == attributesCount)
                    {
                        reference.Rows[i][j] = reference.Rows[i][j];
                    }
                    else if (Convert.ToDouble(reference.Rows[i][j]) == scale[j, 0])
                    {
                        reference.Rows[i][j] = 0.000001;     //do not want to include 0.0
                    }
                    else
                    {
                        double temp = Convert.ToDouble(reference.Rows[i][j]);
                        reference.Rows[i][j] = Math.Abs(temp - scale[j, 0]) / Math.Abs(scale[j, 1] - scale[j, 0]);
                    }
                }
            }
        }
        /// <summary>
        /// Function of GetClasses
        /// </summary>
        /// <remarks>this function will get all the unique class numbers from the class field</remarks>
        /// <param name="reference">the reference datatable</param>
        /// <param name="classField">the number of index of the datatable</param>
        /// <returns>unique values of the classes</returns>
        public IList<int> GetClasses(DataTable reference, int classField)
        {
            IList<int> classNumbers = new List<int>();
            DataTable tempTable = sortTable(reference, classField);  //Sort the table
            int number = 0;
            bool add = true;
            foreach (DataRow dr in tempTable.Rows)
            {
                int tempN = Convert.ToInt16(dr[classField]);
                add = true;
                if (number == 0)
                    add = true;
                else
                {
                    foreach (int CN in classNumbers)
                    {
                        if (CN == tempN)
                        {
                            add = false;
                            break;
                        }
                    }
                }
                if (add)
                    classNumbers.Add(tempN);
                number++;
            }
            return classNumbers;
        }
        /// <summary>
        /// Function of GetClassSamples, get the numbers of samples for each class
        /// </summary>
        /// <param name="reference">A datatable contains both attributes and class field.</param>
        /// <param name="classField">the index of column where the class data were saved.</param>
        /// <returns>A dictionary with class as well as samples</returns>
        public Dictionary<string, int> GetClassSamples(DataTable reference, int classField)
        {
            Dictionary<string, int> classSamples = new Dictionary<string, int>();
            IList<int> classNumbers = new List<int>();
            int number = 0;
            bool add = true;
            foreach (DataRow dr in reference.Rows)
            {
                int tempN = Convert.ToInt16(dr[classField]);
                add = true;
                if (number == 0)
                    add = true;
                else
                {
                    foreach (int CN in classNumbers)
                    {
                        if (CN == tempN)
                        {
                            add = false;
                            break;
                        }
                    }
                }
                if (add)
                    classNumbers.Add(tempN);
                number++;
            }
            foreach (int numb in classNumbers)
            {
                int NumbofSample = countSamples(reference, classField, numb);
                string className = "class" + numb.ToString();
                classSamples.Add(className, NumbofSample);
            }
            return classSamples;
        }
        /// <summary>
        /// Function of countSamples
        /// </summary>
        /// <param name="refTable">a reference table where the attributes and class value were saved.</param>
        /// <param name="FieldIndex">the index of field where the class values was saved</param>
        /// <param name="classNumb">the class number to be counted</param>
        /// <returns>a scalar indicating the numbers of samples belong to that class</returns>
        private int countSamples(DataTable refTable, int FieldIndex, int classNumb)
        {
            int number = 0;
            foreach (DataRow dr in refTable.Rows)
            {
                if (classNumb == Convert.ToInt32(dr[FieldIndex]))
                    number++;
            }
            return number;
        }
        /// <summary>
        /// Function of preprocess
        /// </summary>
        /// <param name="reference">the reference table where data will be preprocessed.</param>
        /// <param name="attributeIndexes">a list of indexes where saved the index for attributes vector and class number</param>
        /// <returns>a datatable recording data only for further analysis</returns>
        public DataTable preprocess(DataTable reference, IList<int> attributeIndexes)
        {
            DataTable startTable = ExtractColumns(reference, attributeIndexes);
            DataTable resultTable = sortTable(startTable, startTable.Columns.Count - 1);
            double[,] scale = FindScale(resultTable);
            Normailize(ref resultTable, scale);
            return resultTable;
        }
        /// <summary>
        /// Function of SVMpreprocess
        /// </summary>
        /// <remarks>preprocess the inputs to create LibSVM format</remarks>
        /// <param name="reference">the datatable saving reference data inputs</param>
        /// <param name="attributeIndexes">the indexes of attributes and classes</param>
        public void SVMpreprocess(DataTable reference, IList<int> attributeIndexes, string trainTextPath)
        {
            DataTable startTable = ExtractColumns(reference, attributeIndexes);
            DataTable resultTable = sortTable(startTable, startTable.Columns.Count - 1);
            double[,] scale = FindScale(resultTable);
            Normailize(ref resultTable, scale);
            Save2LibSVMTrain(resultTable, trainTextPath);
        }
        /// <summary>
        /// Function of Save2LibSVMTrain
        /// </summary>
        /// <remarks>the datatable here were preprocessed which in the format of class, attribute1, attribute2,...attributeN</remarks>
        /// <param name="refTable">the datatable where the records were saved.</param>
        /// <param name="filename">the filename of which the data in the format that LibSVM can recognize were saved.</param>
        private void Save2LibSVMTrain(DataTable refTable, string filename)
        {
            StreamWriter sr;
            if (File.Exists(filename) == false)
            {
                try
                {
                    sr = File.CreateText(filename);
                }
                catch
                {
                    throw new Exception("Unable to create " + filename);
                }
            }
            else { sr = new StreamWriter(filename); }
            for (int j = 0; j < refTable.Rows.Count; j++)
            {
                DataRow dr = refTable.Rows[j];
                int lastIndex = refTable.Columns.Count - 1;
                string tempStr = dr[lastIndex].ToString() + " ";
                for (int i = 0; i < refTable.Columns.Count - 1; i++)
                {
                    tempStr = tempStr + (i + 1).ToString() + ":" + dr[i].ToString() + " ";
                }
                sr.WriteLine(tempStr);
            }
            sr.Close();
        }

        /// <summary>
        /// Get unique values from an array
        /// </summary>
        /// <typeparam name="T">the data type of the array</typeparam>
        /// <param name="array">an array of values</param>
        /// <returns>a vector of unique values</returns>
        private IList<T> GetUniqueValuesFromArray<T>(T[] array)
        {
            IList<T> uniqueValues = new List<T>();
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                bool addValue = true;
                if (i == 0)
                    uniqueValues.Add(array[i]);
                else
                {
                    foreach (T value in uniqueValues)
                    {
                        if (array[i].ToString() == value.ToString())
                        {
                            addValue = false;
                            break;
                        }
                    }
                    if (addValue)
                        uniqueValues.Add(array[i]);
                }
            }
            return uniqueValues;
        }
        /// <summary>
        /// Get an array of values from an array by indexes
        /// </summary>
        /// <typeparam name="T">the data type (could be different data type)</typeparam>
        /// <param name="indexes">the indexes of the value to extract</param>
        /// <returns>a new array contain only value at the certain index locations</returns>
        public T[] GetValuesfromArrayViaIndexes<T>(T[] oldArray, IList<int> indexes)
        {
            T[] newArray = new T[indexes.Count];
            int loop = 0;
            foreach (int index in indexes)
            {
                newArray[loop] = oldArray[index];
                loop++;
            }
            return newArray;
        }

        /// <summary>
        /// function of ConvertDataTable2NormalizedMatrix:
        /// which will convert the datatable to a normalized matrix which can be processed by classifier.
        /// </summary>
        /// <remarks>to be consistent with the image data to convert the datatable</remarks>
        /// <param name="table">the datatable that will be processed</param>
        /// <param name="ranges">the ranges of the attributes</param>
        /// <returns>a normalized matrix where rows represent the data record and column represent attributes</returns>
        private double[,] ConvertDataTable2NormailizedMatrix(DataTable table, double[,] ranges)
        {
            int rowsofTable = table.Rows.Count;
            int colsofTable = table.Columns.Count;
            double[,] result = new double[colsofTable, rowsofTable];
            //Already been normalized or no need for normalization
            Preprocessor prep = new Preprocessor();
            prep.Normailize(ref table, ranges);
            for (int i = 0; i < rowsofTable; i++)
            {
                for (int j = 0; j < colsofTable; j++)
                {
                    result[j, i] = Convert.ToDouble(table.Rows[i][j]);
                }
            }
            return result;
        }

        /// <summary>
        /// Convert a column of a datatable to an array
        /// </summary>
        /// <typeparam name="T">the type of conversion (Double, String)</typeparam>
        /// <param name="dt">a datatable to be converted.</param>
        /// <param name="FieldIndex">the field index of the column</param>
        /// <returns>an array of values</returns>
        public T[] DataColumnAsArray<T>(DataTable dt, int FieldIndex)
        {
            T[] values = new T[dt.Rows.Count];
            int number = 0;
            foreach (DataRow dr in dt.Rows)
            {
                values[number] = (T)Convert.ChangeType(dr[FieldIndex], typeof(T));
                number++;
            }
            return values;
        }

        /// <summary>
        /// Get a list of unique values as well as the numbers of such samples
        /// </summary>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <param name="uniqClassValues">a list of unique class value</param>
        /// <param name="uniqClassSamples">the numbers of samples correspond with each unique class value</param>
        public void GetUniqValuesSamples<T>(T[] classNumbers, out IList<T> uniqClassValues, out IList<int> uniqClassSamples)
        {
            uniqClassValues = new List<T>();
            uniqClassSamples = new List<int>();
            for (int i = 0; i < classNumbers.Length; i++)    //Might be problematic
            {
                bool addValue = true;
                if (i == 0)
                {
                    uniqClassValues.Add(classNumbers[i]);
                    uniqClassSamples.Add(1);
                }
                else
                {
                    int number = 0;
                    foreach (T classValue in uniqClassValues)
                    {
                        if (Convert.ChangeType(classValue, typeof(T)).ToString() == Convert.ChangeType(classNumbers[i], typeof(T)).ToString())
                        {
                            addValue = false;
                            uniqClassSamples[number] += 1;
                            break;
                        }
                        number++;
                    }
                    if (addValue)
                    {
                        uniqClassValues.Add(classNumbers[i]);
                        uniqClassSamples.Add(1);
                    }
                }
            }
        }

        /// <summary>
        /// Convert a list to an array
        /// </summary>
        public T[] convertList2Array<T>(IList<T> list)
        {
            T[] array = new T[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i];
            return array;
        }
        /// <summary>
        /// Convert an array to a list
        /// </summary>
        public IList<T> convertArray2List<T>(T[] array)
        {
            IList<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
                list.Add(array[i]);
            return list;
        }
        /// <summary>
        /// getClassFieldAsArray: get an array of integers from a data table
        /// </summary>
        /// <param name="dt">a reference data table used for classification</param>
        /// <param name="classFieldIndex">the index of the class field</param>
        /// <returns>an array of integers</returns>
        public int[] getClassFieldAsArray(DataTable dt, int classFieldIndex)
        {
            IList<int> conValues = new List<int>();
            foreach (DataRow dr in dt.Rows)
            {
                int tempN = Convert.ToInt16(dr[classFieldIndex]);
                conValues.Add(tempN);
            }
            int[] continousValues = convertList2Array(conValues);
            return continousValues;
        }
        /// <summary>
        /// getContinuousValuesFromDataTable
        /// </summary>
        /// <param name="dt">a datatable</param>
        /// <param name="FieldIndex">the field index</param>
        /// <returns>a vector of double values</returns>
        public double[] getContinousValuesFromDataTable(DataTable dt, int FieldIndex)
        {
            IList<double> conValues = new List<double>();
            foreach (DataRow dr in dt.Rows)
            {
                double tempN = Convert.ToDouble(dr[FieldIndex]);
                conValues.Add(tempN);
            }
            double[] continousValues = convertList2Array(conValues);
            return continousValues;
        }
        /// <summary>
        /// get discrete values from DataTable as an array
        /// </summary>
        /// <param name="dt">a datatable</param>
        /// <param name="FieldIndex">the field index</param>
        /// <returns>a vector of double values</returns>
        public string[] getDiscreteValuesFromDataTable(DataTable dt, int FieldIndex)
        {
            IList<string> disValues = new List<string>();
            foreach (DataRow dr in dt.Rows)
            {
                string tempN = dr[FieldIndex].ToString();
                disValues.Add(tempN);
            }
            string[] discreteValues = convertList2Array(disValues);
            return discreteValues;
        }
        /// <summary>
        /// Randomly extract a certain number of value from an array
        /// </summary>
        /// <typeparam name="T">could be all data type</typeparam>
        /// <param name="array">an array to select value from</param>
        /// <param name="number">number of value to select from (could not be larger than the length of array</param>
        /// <returns>a new array with the extracted values</returns>
        public T[] RandomExtract<T>(T[] array, int number)
        {
            IList<int> checkIndexes = new List<int>();   //Add the check indexes just to avoid selecting samples more than once
            T[] extractValues = new T[number];
            if (number > array.Length)
                throw new Exception("Can not extract " + number.ToString() + " values from an array with " + array.Length.ToString() + " values.");
            else if (number == array.Length)
                return array;
            else
            {
                int extract = 0;
                while (extract < number)
                {
                    Random r = new Random(Guid.NewGuid().GetHashCode());
                    int index = r.Next(array.Length);
                    if (extract == 0)
                    {
                        extractValues[extract] = array[index];
                        checkIndexes.Add(index);
                        extract++;
                    }
                    else
                    {
                        bool addValue = true;
                        foreach (int tempInd in checkIndexes)
                        {
                            if (index == tempInd)     //Means the value has already been included
                            {
                                addValue = false;
                                break;
                            }
                        }
                        if (addValue)
                        {
                            extractValues[extract] = array[index];
                            checkIndexes.Add(index);
                            extract++;
                        }
                    }
                }
            }
            return extractValues;
        }
        /// <summary>
        /// Give a poper name of a new field to be added to the desired data table
        /// </summary>
        /// <param name="dt">the datatable where the field is going to be added</param>
        /// <returns>the name of a new field</returns>
        public string GiveFieldName(DataTable dt)
        {
            string columnName = " ";
            bool addField = false;
            int j = 0;
            while (addField == false)
            {
                if (j == 0)
                    columnName = "Predict";
                else
                    columnName = "Predict" + j.ToString();
                j++;
                foreach (DataColumn dc in dt.Columns)
                {
                    if (dc.Caption == columnName)
                    {
                        addField = false;
                        break;
                    }
                    addField = true;
                }
            }
            return columnName;
        }

        /// <summary>
        /// Randomly extract a certain number of value from an array
        /// </summary>
        /// <typeparam name="T">could be all data type</typeparam>
        /// <param name="array">an array to select value from</param>
        /// <param name="number">number of value to select from (could not be larger than the length of array</param>
        /// <param name="others">the other values which are not randomly selected</param>
        /// <returns>a new array with the extracted values</returns>
        public T[] RandomExtract<T>(T[] array, int number, out IList<T> others)
        {
            others = new List<T>();
            IList<int> checkIndexes = new List<int>();   //Add the check indexes just to avoid selecting samples more than once
            T[] extractValues = new T[number];
            if (number > array.Length)
                throw new Exception("Can not extract " + number.ToString() + " values from an array with " + array.Length.ToString() + " values.");
            else if (number == array.Length)
                return array;
            else
            {
                int extract = 0;
                while (extract < number)
                {
                    Random r = new Random(Guid.NewGuid().GetHashCode());
                    int index = r.Next(array.Length);
                    if (extract == 0)
                    {
                        extractValues[extract] = array[index];
                        checkIndexes.Add(index);
                        extract++;
                    }
                    else
                    {
                        bool addValue = true;
                        foreach (int tempInd in checkIndexes)
                        {
                            if (index == tempInd)     //Means the value has already been included
                            {
                                addValue = false;
                                break;
                            }
                        }
                        if (addValue)
                        {
                            extractValues[extract] = array[index];
                            checkIndexes.Add(index);
                            extract++;
                        }
                    }
                }
            }
            for (int i = 0; i < array.Length; i++)
            {
                bool add = true;
                foreach (int index in checkIndexes)
                {
                    if (i == index)
                    {
                        add = false;
                        break;
                    }
                }
                if (add)
                    others.Add(array[i]);
            }
            return extractValues;
        }
        
        /// <summary>
        /// Check if a value is in an array
        /// </summary>
        /// <typeparam name="T">data type</typeparam>
        /// <param name="Value">a value</param>
        /// <param name="array">an array of values</param>
        /// <returns>true if the value is in the array false otherwise</returns>
        public bool contains<T>(T Value, T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (Value.ToString() == array[i].ToString())
                    return true;
            }
            return false;
        }

    }
}
