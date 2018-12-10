using System;

namespace AddFileToProject
{
    public class CmdArgExtractor
    {
        private string[] _validPrefixes;
        private string _argsSep;
        private string _argsValSep;

        public CmdArgExtractor(string[] validPrefixes, string argsSep, string argsValSep)
        {
            if (validPrefixes.Length == 0)
            {
                throw new Exception("validPrefixes array has no values.");
            }

            if (argsValSep.ToString().Trim().Length == 0)
            {
                throw new Exception("argsValSep cannot be blank.");
            }

            if (argsValSep.ToString().Trim().Length == 0)
            {
                throw new Exception("argsSep cannot be blank.");
            }

            this._validPrefixes = validPrefixes;
            this._argsSep = argsSep;
            this._argsValSep = argsValSep;
        }

        public CmdArgExtractor(string argsSep)
        {
            if (argsSep.ToString().Trim() == null)
            {
                throw new Exception("argsSep cannot be blank.");
            }
            this._argsSep = argsSep;
        }

        public bool ValidArgsPrefixes(string[] args)
        {
            bool retVal = false;

            if (this._validPrefixes.Length == 0)
            {
                throw new Exception("validPrefixes array has no values.");
            }

            for (int i = 0; i < this._validPrefixes.Length; i++)
            {
                //this._validPrefixes[i] = this._validPrefixes[i].ToLower();

                if (i == args.Length)
                {
                    break;
                }

                for (int k = 0; k < args.Length; k++)
                {
                    //args[k] = args[k].ToLower();

                    if (args[k].Contains(this._validPrefixes[i]))
                    {
                        retVal = true;
                        break;
                    }
                    else
                    {
                        retVal = false;
                    }
                }

                if (retVal == true)
                {
                    continue;
                }
                else
                {
                    break;
                }
            }
            return retVal;
        }

        public string[] GetArgValues(string[] args)
        {
            string[] retArgs = new string[args.Length];
            string[] tmpArr;
            string[] separators = new string[] {this._argsSep, this._argsValSep};

            for (int i = 0; i < retArgs.Length; i++)
            {
                tmpArr = args[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);
                retArgs[i] = tmpArr[1];
            }

            return retArgs;
        }

        public string[,] GetArgsTwoDimArray(string[] args)
        {
            int firstDim = 2;
            string[,] retArgs = new string[firstDim, args.Length];
            string[] tmpArr;

            string[] separators = new string[] {this._argsSep, this._argsValSep};

            for (int i = 0; i < args.Length; i++)
            {
                tmpArr = args[i].Split(separators, StringSplitOptions.RemoveEmptyEntries);

                retArgs[0,i] = tmpArr[0];
                retArgs[1,i] = tmpArr[1];
            }
            return retArgs;
        }
    }
}
