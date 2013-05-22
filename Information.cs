using System;
using System.Collections.Generic;
using System.Text;

using ARSIE.DataIO;

namespace ARSIE.CART
{
    /// <summary>
    /// Calculate information gain, gain ratio, gini information as well as standard deviation for regression.
    /// </summary> 
    public class Information
    {
        private double[] __continuousAttribute;
        private string[] __discreteAttribute;
        private byte[] __classNumber;
        private double[] __dependentVariable;

        #region CONSTRUCTOR
        /// <summary>
        /// Default constructor
        /// </summary>
        public Information() { }
        /// <summary>
        /// Overloading constructor
        /// </summary>
        /// <param name="continuousAttribute">An array of continuous attribute</param>
        /// <param name="classLabel">an array of class numbers</param>
        public Information(double[] continuousAttribute, byte[] classLabel)
        {
            __continuousAttribute = continuousAttribute;
            __classNumber = classLabel;
        }
        /// <summary>
        /// Overloading constructor
        /// </summary>
        /// <param name="discreteAttribute">An array of discrete attribute</param>
        /// <param name="classLabel">an array of class numbers</param>
        public Information(string[] discreteAttribute, byte[] classLabel)
        {
            __discreteAttribute = discreteAttribute;
            __classNumber = classLabel;
        }
        /// <summary>
        /// Overloading constructor
        /// </summary>
        /// <param name="discreteAttribute">An array of continuous attribute</param>
        /// <param name="classLabel">an array of dependent values</param>
        public Information(double[] continuousAttribute, double[] dependentVariable)
        {
            __continuousAttribute = continuousAttribute;
            __dependentVariable = dependentVariable;
        }
        #endregion


        #region FUNCTIONS
        #region Continuous Attribute
        public double CalcGiniIndex(double[] continuousAttribute, byte[] classNumbers, bool sort, out double splitValue)
        {
            if (continuousAttribute.Length != classNumbers.Length)
                throw new Exception("Not able to calculate entropy values with different numbers of records for attribute and class labels.");
            splitValue = 0.0;
            return CalcAttributeGini(continuousAttribute, classNumbers, sort, out splitValue);
        }

        public double CalcGiniIndex(string[] attribute, byte[] classNumbers)
        {
            if (attribute.Length != classNumbers.Length)
                throw new Exception("Not able to calculate entropy values with different numbers of records for attribute and class labels.");
            return CalcAttributeGini(attribute, classNumbers);
        }
        /// <summary>
        /// Calculate the information gain value
        /// </summary>
        /// <param name="continuousAttribute">a list of continuous attribute value</param>
        /// <param name="classNumbers">an array of class numbers</param>
        /// <returns>the information gain value</returns>
        public double CalcInformationGain(double[] continuousAttribute, byte[] classNumbers, bool sort, out double splitValue)
        {
            if (continuousAttribute.Length != classNumbers.Length)
                throw new Exception("Not able to calculate entropy values with different numbers of records for attribute and class labels.");
            double classEntropy = CalcClassEntropy(classNumbers);
            splitValue = 0.0;
            double attributeEntropy = CalcAttributeEntropy(continuousAttribute, classNumbers, sort, out splitValue);
            return classEntropy - attributeEntropy;
        }

        /// <summary>
        /// Calculate the information gain value
        /// </summary>
        /// <param name="continuousAttribute">a list of continuous attribute value</param>
        /// <param name="classNumbers">an array of class numbers</param>
        /// <returns>the information gain value</returns>
        public double CalcInformationGain(string[] discreteAttribute, byte[] classNumbers)
        {
            if (discreteAttribute.Length != classNumbers.Length)
                throw new Exception("Not able to calculate entropy values with different numbers of records for attribute and class labels.");
            double classEntropy = CalcClassEntropy(classNumbers);
            double attributeEntropy = CalcAttributeEntropy(discreteAttribute, classNumbers);
            return classEntropy - attributeEntropy;
        }

        /// <summary>
        /// Calculate the gain ratio value
        /// </summary>
        /// <param name="continuousAttribute">a list of continuous attribute value</param>
        /// <param name="classNumbers">an array of class numbers</param>
        /// <param name="splitValue">the split value which result in the smallest attribute entropy value to maximize the information gain value</param>
        /// <returns>the gain ratio value</returns>
        public double CalcGainRatio(double[] continuousAttribute, byte[] classNumbers, bool sort, out double splitValue)
        {
            double classEntropy = CalcClassEntropy(classNumbers);
            double attributeEntropy = CalcAttributeEntropy(continuousAttribute, classNumbers, sort, out splitValue);
            double EntropyGain = classEntropy - attributeEntropy;
            double attributeSplitInfo = CalcSplitInfo(continuousAttribute, splitValue);
            double EntropyRatio = EntropyGain / attributeSplitInfo;
            return EntropyRatio;
        }

        /// <summary>
        /// Calculate the gain ratio value
        /// </summary>
        /// <param name="continuousAttribute">a list of discrete attribute value</param>
        /// <param name="classNumbers">an array of class numbers</param>
        /// <returns>the gain ratio value</returns>
        public double CalcGainRatio(string[] discreteAttribute, byte[] classNumbers)
        {
            double classEntropy = CalcClassEntropy(classNumbers);
            double attributeEntropy = CalcAttributeEntropy(discreteAttribute, classNumbers);
            double EntropyGain = classEntropy - attributeEntropy;
            double attributeSplitInfo = CalcSplitInfo(discreteAttribute);
            double EntropyRatio = EntropyGain / attributeSplitInfo;
            return EntropyRatio;
        }

        /// <summary>
        ///This function will calculate the continuous Attribute's Entropy value 
        /// </summary>
        /// <param name="attributes">a list of continuous attribute value</param>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <param name="split_value">the split value which result in the smallest attribute entropy value to maximize the information gain value</param>
        /// <returns>the entropy vlaue for a certain continuous attribute</returns>
        public double CalcAttributeEntropy(double[] oldattributes, byte[] oldclassNumbers, bool sort, out double split_value)
        {
            //Sort the Attributes
            byte[] classNumbers = (byte[])oldclassNumbers.Clone();
            double[] attributes = (double[])oldattributes.Clone();
            if (sort)
            {
                attributes = SortAttributeAndClass<byte>(oldattributes, oldclassNumbers, out classNumbers);
            }
            //double[] attributes = SortAttributeAndClass(oldattributes, oldclassNumbers, out classNumbers);

            IList<byte> uniqClassNumbers = GetUniqueValues<byte>(classNumbers);
            double minEntropy = 0.0;
            long total = classNumbers.Length;
            split_value = 0.0;
            for (int i = 0; i < attributes.Length - 1; i++)
            {
                double splitValue = (attributes[i] + attributes[i + 1]) / 2; 
                IList<byte> subsetClass = new List<byte>();
                //Greater than or equal to 
                splitVectorByScalar(attributes, classNumbers, splitValue, true, out subsetClass);
                byte[] GEclassArray = convertList2Array(subsetClass);
                IList<byte> uniqClass = new List<byte>();
                IList<int> uniqClassSamples = new List<int>();
                GetUniqValuesSamples(GEclassArray, out uniqClass, out uniqClassSamples);
                double greaterEntropy = getEntropyValuefromProbability(uniqClassSamples);
                long subTotal = 0;
                foreach (int value in uniqClassSamples)
                    subTotal += value;
                greaterEntropy = greaterEntropy * ((subTotal * 1.0) / total);
                //Less than
                splitVectorByScalar(attributes, classNumbers, splitValue, false, out subsetClass);
                byte[] LTclassArray = convertList2Array(subsetClass);
                GetUniqValuesSamples(LTclassArray, out uniqClass, out uniqClassSamples);
                double lessEntropy = getEntropyValuefromProbability(uniqClassSamples);
                subTotal = 0;
                foreach (int value in uniqClassSamples)
                    subTotal += value;
                lessEntropy = lessEntropy * ((subTotal * 1.0) / total);
                double tempEntropy = greaterEntropy + lessEntropy;
                if (i == 0)
                {
                    minEntropy = tempEntropy;
                    split_value = splitValue;
                }
                else if (tempEntropy < minEntropy)
                {
                    minEntropy = tempEntropy;
                    split_value = splitValue;
                }
            }
            return minEntropy;
        }

        /// <summary>
        ///This function will calculate the continuous Attribute's gini value
        /// </summary>
        /// <param name="attributes">a list of continuous attribute value</param>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <param name="split_value">the split value which result in the smallest attribute entropy value to maximize the information gain value</param>
        /// <returns>the entropy vlaue for a certain continuous attribute</returns>
        public double CalcAttributeGini(double[] oldattributes, byte[] oldclassNumbers, bool sort, out double split_value)
        {
            //Sort the Attributes
            byte[] classNumbers = (byte[])oldclassNumbers.Clone();
            double[] attributes = (double[])oldattributes.Clone();
            if (sort)
            {
                attributes = SortAttributeAndClass<byte>(oldattributes, oldclassNumbers, out classNumbers);
            }
            //double[] attributes = SortAttributeAndClass(oldattributes, oldclassNumbers, out classNumbers);

            IList<byte> uniqClassNumbers = GetUniqueValues<byte>(classNumbers);
            double minEntropy = 0.0;
            long total = classNumbers.Length;
            split_value = 0.0;
            for (int i = 0; i < attributes.Length - 1; i++)
            {
                double splitValue = (attributes[i] + attributes[i + 1]) / 2;
                IList<byte> subsetClass = new List<byte>();
                //Greater than or equal to 
                splitVectorByScalar(attributes, classNumbers, splitValue, true, out subsetClass);
                byte[] GEclassArray = convertList2Array(subsetClass);
                IList<byte> uniqClass = new List<byte>();
                IList<int> uniqClassSamples = new List<int>();
                GetUniqValuesSamples(GEclassArray, out uniqClass, out uniqClassSamples);
                double greaterEntropy = getGiniValuefromProbability(uniqClassSamples);
                long subTotal = 0;
                foreach (int value in uniqClassSamples)
                    subTotal += value;
                greaterEntropy = greaterEntropy * ((subTotal * 1.0) / total);
                //Less than
                splitVectorByScalar(attributes, classNumbers, splitValue, false, out subsetClass);
                byte[] LTclassArray = convertList2Array(subsetClass);
                GetUniqValuesSamples(LTclassArray, out uniqClass, out uniqClassSamples);
                double lessEntropy = getGiniValuefromProbability(uniqClassSamples);
                subTotal = 0;
                foreach (int value in uniqClassSamples)
                    subTotal += value;
                lessEntropy = lessEntropy * ((subTotal * 1.0) / total);
                double tempEntropy = greaterEntropy + lessEntropy;
                if (i == 0)
                {
                    minEntropy = tempEntropy;
                    split_value = splitValue;
                }
                else if (tempEntropy < minEntropy)
                {
                    minEntropy = tempEntropy;
                    split_value = splitValue;
                }
            }
            return minEntropy;
        }
        /// <summary>
        /// Very time consuming...
        /// Need to use other sorting methods.
        /// </summary>
        internal double[] SortAttributeAndClass<T>(double[] oldattributes, T[] oldClassNumbers, out T[] classNumbers)
        {
            double[] attributes = (double[])oldattributes.Clone();
            classNumbers = (T[])oldClassNumbers.Clone();

            for (int i = 0; i < attributes.Length; i++)
            {
                for (int j = i; j < attributes.Length; j++)
                {
                    double tempValue = 0.0;
                    T tempClass = default(T);

                    if (attributes[i] > attributes[j])
                    {
                        tempValue = attributes[i];
                        attributes[i] = attributes[j];
                        attributes[j] = tempValue;

                        tempClass = classNumbers[i];
                        classNumbers[i] = classNumbers[j];
                        classNumbers[j] = tempClass;
                    }
                }
            }
            return attributes;
        }
        /// <summary>
        /// This function will calculate the class entropy vlaue
        /// </summary>
        /// <param name="classNumbers">an array of class numbers</param>
        /// <returns>the clas entropy value</returns>
        public double CalcClassEntropy(byte[] classNumbers)
        {
            IList<byte> classValueList = new List<byte>();
            IList<int> classSampleList = new List<int>();
            GetUniqValuesSamples(classNumbers, out classValueList, out classSampleList);
            double labelEnVal = getEntropyValuefromProbability(classSampleList);
            return labelEnVal;
        }

        /// <summary>
        /// This function is used to calculate the split info which will be used for calculating gain ratio
        /// </summary>
        /// <param name="attributes">a list of continuous attribute value</param>
        /// <param name="split_Value">the split value which result in the smallest attribute entropy value to maximize the information gain value</param>
        /// <returns>the split information</returns>
        public double CalcSplitInfo(double[] attributes, double split_Value)
        {
            long largerCount = 0;
            long smallerCount = 0;
            for (int i = 0; i < attributes.Length; i++)
            {
                if (attributes[i] >= split_Value)
                    largerCount++;
                else
                    smallerCount++;
            }
            long total = largerCount + smallerCount;
            double result = -((largerCount * 1.0) / total) * Math.Log((largerCount * 1.0) / total, 2.0) - ((smallerCount * 1.0) / total) * Math.Log((smallerCount * 1.0) / total, 2.0);
            return result;
        }
        #endregion

        #region Discrete Attribute
        /// <summary>
        /// This function will calculate the discrete Attribute's Entropy value
        /// </summary>
        /// <param name="attributes">an array of discrete attribute value</param>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <returns>the entropy value</returns>
        public double CalcAttributeEntropy(string[] attributes, byte[] classNumbers)
        {
            IList<string> uniqValues = new List<string>();
            IList<int> samplesCount = new List<int>();
            GetUniqValuesSamples<string>(attributes, out uniqValues, out samplesCount);
            double AttributeEntro = 0.0;
            int loop = 0;
            foreach (string uniqValue in uniqValues)
            {
                IList<int> tempClass = new List<int>();
                for (int i = 0; i < attributes.Length; i++)
                {
                    if (attributes[i] == uniqValue)
                        tempClass.Add(classNumbers[i]);
                }
                IList<int> tempUniqClass = new List<int>();
                IList<int> tempSamples = new List<int>();
                int[] tempClassArray = convertList2Array(tempClass);
                GetUniqValuesSamples<int>(tempClassArray, out tempUniqClass, out tempSamples);
                double tempEntro = getEntropyValuefromProbability(tempSamples);
                AttributeEntro += tempEntro * (samplesCount[loop] / attributes.Length);
                loop++;
            }
            return AttributeEntro;
        }

        /// <summary>
        /// This function will calculate the discrete Attribute's Entropy value
        /// </summary>
        /// <param name="attributes">an array of discrete attribute value</param>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <returns>the entropy value</returns>
        public double CalcAttributeGini(string[] attributes, byte[] classNumbers)
        {
            IList<string> uniqValues = new List<string>();
            IList<int> samplesCount = new List<int>();
            GetUniqValuesSamples<string>(attributes, out uniqValues, out samplesCount);
            double AttributeEntro = 1.0;
            long total = classNumbers.Length;

            CombinationGenerator<string> cg = new CombinationGenerator<string>();
            IEnumerable<List<string>> potentialCombinations = cg.ProduceWithoutRecursion(uniqValues as List<string>);
            foreach (List<string> combination in potentialCombinations)
            {
                if (combination.Count != 0 && combination.Count == uniqValues.Count)
                {
                    IList<byte> subsetClass = new List<byte>();
                    //in
                    splitVectorByContainage(attributes, classNumbers, combination, true, out subsetClass);
                    byte[] inClassArray = convertList2Array(subsetClass);
                    IList<byte> uniqClass = new List<byte>();
                    IList<int> uniqClassSamples = new List<int>();
                    GetUniqValuesSamples(inClassArray, out uniqClass, out uniqClassSamples);
                    double inEntropy = getGiniValuefromProbability(uniqClassSamples);
                    long subTotal = 0;
                    foreach (int value in uniqClassSamples)
                        subTotal += value;
                    inEntropy = inEntropy * ((subTotal * 1.0) / total);
                    //Not in
                    splitVectorByContainage(attributes, classNumbers, combination, true, out subsetClass);
                    byte[] notInClassArray = convertList2Array(subsetClass);
                    uniqClass = new List<byte>();
                    uniqClassSamples = new List<int>();
                    GetUniqValuesSamples(inClassArray, out uniqClass, out uniqClassSamples);
                    double notInEntropy = getGiniValuefromProbability(uniqClassSamples);
                    subTotal = 0;
                    foreach (int value in uniqClassSamples)
                        subTotal += value;
                    notInEntropy = notInEntropy * ((subTotal * 1.0) / total);
                    if (inEntropy + notInEntropy < AttributeEntro)
                        AttributeEntro = inEntropy + notInEntropy;
                }
            }
            return AttributeEntro;
        }

        /// <summary>
        /// This function is used to calculate the split info which will be used for calculating gain ratio
        /// </summary>
        /// <param name="attributes">a list of discrete attribute value</param>
        /// <param name="split_Value">the split value which result in the smallest attribute entropy value to maximize the information gain value</param>
        /// <returns>the split information</returns>
        public double CalcSplitInfo(string[] attributes)
        {
            IList<string> uniqValues = new List<string>();
            IList<int> uniqSamples = new List<int>();
            GetUniqValuesSamples<string>(attributes, out uniqValues, out uniqSamples);
            return getEntropyValuefromProbability(uniqSamples);
        }
        #endregion


        /// <summary>
        /// Split a list of continuous values (double) into two either greater than or equal to certain split value or smaller than certain split value
        /// </summary>
        /// <param name="attributes">a list of continuous attribute value</param>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <param name="splitValue">a scalar of split value</param>
        /// <param name="LargerorEqual">indicator of weather select the upper part or lower part</param>
        /// <param name="subsetClassNumbers">the corresponding class numbers relevant to the split vector</param>
        internal void splitVectorByScalar(double[] attributes, byte[] classNumbers, double splitValue, bool LargerorEqual, out IList<byte> subsetClassNumbers)
        {
            subsetClassNumbers = new List<byte>();
            for (int i = 0; i < attributes.Length; i++)
            {
                if (LargerorEqual) //Larger than or equal to
                {
                    if (attributes[i] >= splitValue)
                        subsetClassNumbers.Add(classNumbers[i]);
                }
                else       //Smaller than
                {
                    if (attributes[i] < splitValue)
                        subsetClassNumbers.Add(classNumbers[i]);
                }
            }
        }

        /// <summary>
        /// Split vector by containage information
        /// </summary>
        /// <param name="attributes">a list of continuous attribute value</param>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <param name="combination">a list of strings</param>
        /// <param name="contains">true to find all items within the combination, false otherwise</param>
        /// <param name="subsetClassNumbers">the corresponding class numbers relevant to the contain condition</param>
        internal void splitVectorByContainage(string[] attributes, byte[] classNumbers, List<string> combination, bool contains, out IList<byte> subsetClassNumbers)
        {
            subsetClassNumbers = new List<byte>();
            Preprocessor prep = new Preprocessor();
            
            for (int i = 0; i < attributes.Length; i++)
            {
                if (contains) //Larger than or equal to
                {
                    if (prep.contains<string>(attributes[i], combination.ToArray()))
                        subsetClassNumbers.Add(classNumbers[i]);
                }
                else       //Smaller than
                {
                    if (!prep.contains<string>(attributes[i], combination.ToArray()))
                        subsetClassNumbers.Add(classNumbers[i]);
                }
            }
        }

        /// <summary>
        /// Get a list of unique values from an array of class numbers
        /// </summary>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <returns>a list of unique class value</returns>
        internal IList<T> GetUniqueValues<T>(T[] classNumbers)
        {
            IList<T> classList = new List<T>();
            for (int i = 0; i < classNumbers.Length - 1; i++)    //Might be problematic
            {
                bool addValue = true;
                if (i == 0)
                {
                    classList.Add(classNumbers[i]);
                }
                else
                {
                    foreach (T classValue in classList)
                    {
                        if (Convert.ChangeType(classValue, typeof(T)) == Convert.ChangeType(classNumbers[i], typeof(T)))
                        {
                            addValue = false;
                            break;
                        }
                    }
                    if (addValue)
                    {
                        classList.Add(classNumbers[i]);
                    }
                }
            }
            return classList;
        }

        /// <summary>
        /// Get a list of unique values as well as the numbers of such samples
        /// </summary>
        /// <param name="classNumbers">a list of class numbers</param>
        /// <param name="uniqClassValues">a list of unique class value</param>
        /// <param name="uniqClassSamples">the numbers of samples correspond with each unique class value</param>
        internal void GetUniqValuesSamples<T>(T[] classNumbers, out IList<T> uniqClassValues, out IList<int> uniqClassSamples)
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
        /// Calculate the entropy value based on probability information
        /// </summary>
        /// <param name="classNumberSamples">a list of class numbers, such as [1, 2, 1,1,3,2,3,2,1]</param>
        /// <returns>the entropy value</returns>
        /// <see>http://en.wikipedia.org/wiki/C4.5_algorithm#Pseudocode</see>
        internal double getEntropyValuefromProbability(IList<int> classNumberSamples)
        {
            int total = 0;
            foreach (int sampleNumber in classNumberSamples)
                total += sampleNumber;
            double entropy = 0.0;
            foreach (int value in classNumberSamples)
            {
                entropy -= ((value * 1.0) / total) * Math.Log((value * 1.0) / total, 2);
            }
            return entropy;
        }

        /// <summary>
        /// Calculate the entropy value based on probability information
        /// </summary>
        /// <param name="classNumberSamples">a list of class numbers, such as [1, 2, 1,1,3,2,3,2,1]</param>
        /// <returns>the entropy value</returns>
        /// <see>http://en.wikipedia.org/wiki/C4.5_algorithm#Pseudocode</see>
        internal double getGiniValuefromProbability(IList<int> classNumberSamples)
        {
            int total = 0;
            foreach (int sampleNumber in classNumberSamples)
                total += sampleNumber;
            double entropy = 0.0;
            foreach (int value in classNumberSamples)
            {
                entropy += (value * 1.0 / total)*(value*1.0/total);
            }
            return 1.0 - entropy;
        }

        internal T[] convertList2Array<T>(IList<T> list)
        {
            T[] array = new T[list.Count];
            for (int i = 0; i < list.Count; i++)
                array[i] = list[i];
            return array;
        }

        #region   REGRESSION
        /// <summary>
        /// Calculate the variance for splitting an independent variable
        /// </summary>
        /// <param name="independentVariable">an array containing the independent variable</param>
        /// <param name="dependentVariable">an array containing the dependent variable</param>
        /// <param name="splitValue">the split value which resulting the smallest variance value</param>
        /// <returns>the smallest variance value</returns>
        public double CalcVariance(double[] oldIndependentVariable, double[] oldDependentVariable, out double splitValue)
        {
            double[] dependentVariable = (double[])oldDependentVariable.Clone();
            double[] independentVariable = SortAttributeAndClass<double>(oldIndependentVariable, oldDependentVariable, out dependentVariable);

            //Maybe problematic (need to be sort first)
            double variance = 0.0;
            splitValue = 0.0;
            int totalRecords = independentVariable.Length;
            for (int i = 0; i < independentVariable.Length - 1; i++)
            {
                //Get the temporary split value
                if (i == 0)
                {
                    splitValue = (independentVariable[i] + independentVariable[i + 1]) / 2;
                    IList<int> upperIndexes = GetConditionalIndexFromArray(independentVariable, splitValue, "GE");
                    double[] upperDepValues = GetValuesFromArrayViaIndexes<double>(dependentVariable, upperIndexes);
                    double upperPartVariance = getVariance(upperDepValues);

                    IList<int> lowerIndexes = GetConditionalIndexFromArray(independentVariable, splitValue, "LT");
                    double[] lowerDepValues = GetValuesFromArrayViaIndexes<double>(dependentVariable, lowerIndexes);
                    double lowerPartVariance = getVariance(lowerDepValues);
                    variance = lowerPartVariance * (lowerDepValues.Length * 1.0 / (totalRecords * 1.0)) + upperPartVariance * (upperDepValues.Length * 1.0 / (totalRecords * 1.0));
                }
                else
                {
                    double tempVariance = 0.0;
                    double tempsplitValue = (independentVariable[i] + independentVariable[i + 1]) / 2;
                    IList<int> upperIndexes = GetConditionalIndexFromArray(independentVariable, tempsplitValue, "GE");
                    double[] upperDepValues = GetValuesFromArrayViaIndexes<double>(dependentVariable, upperIndexes);
                    double upperPartVariance = getVariance(upperDepValues);

                    IList<int> lowerIndexes = GetConditionalIndexFromArray(independentVariable, tempsplitValue, "LT");
                    double[] lowerDepValues = GetValuesFromArrayViaIndexes<double>(dependentVariable, lowerIndexes);
                    double lowerPartVariance = getVariance(lowerDepValues);
                    tempVariance = lowerPartVariance * (lowerDepValues.Length * 1.0 / (totalRecords * 1.0)) + upperPartVariance * (upperDepValues.Length * 1.0 / (totalRecords * 1.0));
                    if (tempVariance < variance)
                    {
                        variance = tempVariance;
                        splitValue = tempsplitValue;
                    }
                }
            }
            return variance;
        }

        /// <summary>
        /// Calculate the variance for splitting an independent variable
        /// </summary>
        /// <param name="independentVariable">an array containing the discrete independent variable</param>
        /// <param name="dependentVariable">an array containing the dependent variable</param>
        /// <returns>the variance value</returns>
        public double CalcVariance(string[] independentVariable, double[] dependentVariable)
        {
            IList<string> uniqValues = new List<string>();
            IList<int> samplesCount = new List<int>();
            GetUniqValuesSamples<string>(independentVariable, out uniqValues, out samplesCount);
            double variance = 0.0;
            Preprocessor prep = new Preprocessor();
            int totalSamaples = dependentVariable.Length;

            foreach (string uniqValue in uniqValues)
            {
                IList<int> tempIndexes = new List<int>();
                for (int i = 0; i < independentVariable.Length; i++)
                {
                    if (independentVariable[i] == uniqValue)
                        tempIndexes.Add(i);
                }
                double[] tempDepVariable = GetValuesFromArrayViaIndexes<double>(dependentVariable, tempIndexes);
                double tempVariance = getVariance(tempDepVariable);
                variance += tempVariance * (tempDepVariable.Length / (totalSamaples * 1.0));
            }
            return variance;
        }

        /// <summary>
        /// Get a list of value from an array satisfy certain condition from an array
        /// </summary>
        /// <param name="array">an array of data</param>
        /// <param name="specificValue">a specific data to compare with</param>
        /// <param name="indicator">an indicator to reveal the relationship (could be GT, GE, EQ, LE, LT (From IDL^_^))</param>
        /// <returns>an array of indexes whose value at the position satisfy the above condition</returns>
        private IList<int> GetConditionalIndexFromArray(double[] array, double specificValue, string indicator)
        {
            IList<int> indexes = new List<int>();
            for (int i = 0; i < array.Length; i++)
            {
                switch (indicator)
                {
                    case "GT":
                        if (array[i] > specificValue)
                            indexes.Add(i);
                        break;
                    case "GE":
                        if (array[i] >= specificValue)
                            indexes.Add(i);
                        break;
                    case "EQ":
                        if (array[i] == specificValue)
                            indexes.Add(i);
                        break;
                    case "LE":
                        if (array[i] <= specificValue)
                            indexes.Add(i);
                        break;
                    case "LT":
                        if (array[i] < specificValue)
                            indexes.Add(i);
                        break;
                    default:
                        throw new Exception("No such relationship!");
                }
            }
            return indexes;
        }
        /// <summary>
        /// Get a list of values from an array using indexes information
        /// </summary>
        /// <typeparam name="T">different data types are allowed</typeparam>
        /// <param name="array">an array of values</param>
        /// <param name="indexes">a list of indexes</param>
        /// <returns>a new array</returns>
        private T[] GetValuesFromArrayViaIndexes<T>(T[] array, IList<int> indexes)
        {
            T[] result = new T[indexes.Count];
            int loop = 0;
            foreach (int index in indexes)
            {
                result[loop] = array[index];
                loop++;
            }
            return result;
        }
        /// <summary>
        ///Calculate the variance from an array of data 
        /// </summary>
        /// <param name="array">an array of values</param>
        /// <returns>the variance value (sum(Y-avg(Y))^2)</returns>
        public double getVariance(double[] array)
        {
            double avg = average(array);
            double variance = 0.0;
            for (int i = 0; i < array.Length; i++)
            {
                variance = variance + (array[i] - avg) * (array[i] - avg);
            }
            return variance;
        }

        /// <summary>
        ///calculate the average value of an array 
        /// </summary>
        /// <param name="array">an array of values</param>
        /// <returns>the average value of the array</returns>
        private double average(double[] array)
        {
            double sum = 0.0;
            double length = array.Length;
            for (int i = 0; i < length; i++)
                sum = sum + array[i];
            return sum / length;
        }
        #endregion
        #endregion
    }
}
