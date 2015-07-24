using System;
using System.Collections.Generic;
using System.Text;
namespace WebkologTextEditor
{
    namespace Exceptions
    {
        class InvalidInputException : System.Exception { }
        class ValueDoesntExistException : System.Exception { }
        class KeyDoesntExistException : System.Exception { }
    }
    class Configuration
    {
        System.Collections.Hashtable Keys;
        System.Collections.Hashtable Values;
        public Configuration()
        {
            Keys = new System.Collections.Hashtable();
            Values = new System.Collections.Hashtable();
        }
        public Configuration(System.IO.TextReader tr)
        {
            Keys = new System.Collections.Hashtable();
            Values = new System.Collections.Hashtable();
            Parse(tr);
        }
        public string GetValue(string Key, string ValueName)
        {
            System.Collections.Hashtable KeyTable = (System.Collections.Hashtable)Keys[Key];
            if (KeyTable != null)
                if (KeyTable.ContainsKey(ValueName))
                    return (string)KeyTable[ValueName];
                else
                    throw new Exceptions.ValueDoesntExistException();
            else
                throw new Exceptions.KeyDoesntExistException();
        }
        public string GetValue(string ValueName)
        {
            if (Values.ContainsKey(ValueName))
                return (string)Values[ValueName];
            else
                throw new Exceptions.ValueDoesntExistException();
        }
        bool KeyExists(string KeyName) { return Keys.Contains(KeyName); }
        bool ValueExists(string KeyName, string ValueName)
        {
            System.Collections.Hashtable KeyTable = (System.Collections.Hashtable)Keys[KeyName];
            if (KeyTable != null)
                return KeyTable.Contains(ValueName);
            else
                throw new Exceptions.KeyDoesntExistException();
        }
        bool ValueExists(string ValueName) { return Values.Contains(ValueName); }
        public void SetValue(string Key, string ValueName, string Value)
        {
            System.Collections.Hashtable KeyTable = (System.Collections.Hashtable)Keys[Key];
            if (KeyTable != null)
                KeyTable[ValueName] = Value;
            else
                throw new Exceptions.KeyDoesntExistException();
        }
        public void SetValue(string ValueName, string Value) { Values[ValueName] = Value; }
        public void AddKey(string NewKey)
        {
            System.Collections.Hashtable New = new System.Collections.Hashtable();
            Keys[NewKey] = New;
        }
        public void Save(System.IO.TextWriter sw)
        {
            System.Collections.IDictionaryEnumerator Enumerator = Values.GetEnumerator();
            sw.WriteLine("; The values in this group");
            while (Enumerator.MoveNext())
                sw.WriteLine("{0} = {1}", Enumerator.Key, Enumerator.Value);
            sw.WriteLine("; This is where the keys begins");
            Enumerator = Keys.GetEnumerator();
            while (Enumerator.MoveNext())
            {
                System.Collections.IDictionaryEnumerator Enumerator2nd = ((System.Collections.Hashtable)Enumerator.Value).GetEnumerator();
                sw.WriteLine("[{0}]", Enumerator.Key);
                while (Enumerator2nd.MoveNext())
                    sw.WriteLine("{0} = {1}", Enumerator2nd.Key, Enumerator2nd.Value);
            }
        }
        private void Parse(System.IO.TextReader sr)
        {
            System.Collections.Hashtable CurrentKey = null;
            string Line, ValueName, Value;
            while (null != (Line = sr.ReadLine()))
            {
                int j, i = 0;
                while (Line.Length > i && Char.IsWhiteSpace(Line, i)) i++;
                if (Line.Length <= i)
                    continue;
                if (Line[i] == ';')
                    continue;
                if (Line[i] == '[')
                {
                    string KeyName;
                    j = Line.IndexOf(']', i);
                    if (j == -1)
                        throw new Exceptions.InvalidInputException();
                    KeyName = Line.Substring(i + 1, j - i - 1).Trim();
                    if (!Keys.ContainsKey(KeyName))
                        this.AddKey(KeyName);
                    CurrentKey = (System.Collections.Hashtable)Keys[KeyName];
                    while (Line.Length > ++j && Char.IsWhiteSpace(Line, j))
                        if (Line.Length > j)
                        {
                            if (Line[j] != ';')
                                throw new Exceptions.InvalidInputException();
                        }
                    continue;
                }
                j = Line.IndexOf('=', i);
                if (j == -1)
                    throw new Exceptions.InvalidInputException();
                ValueName = Line.Substring(i, j - i).Trim();
                if ((i = Line.IndexOf(';', j + 1)) != -1)
                    Value = Line.Substring(j + 1, i - (j + 1)).Trim();
                else
                    Value = Line.Substring(j + 1).Trim();
                if (CurrentKey != null)
                    CurrentKey[ValueName] = Value;
                else
                    Values[ValueName] = Value;
            }
        }
    }
}