using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AutoRegister
{
    class JsonPasswordRepository
    {
        private string filename = string.Empty;
        private readonly PasswordLedger ledger = null;

        public JsonPasswordRepository(string filename)
        {
            this.filename = filename;

            if(!File.Exists(filename))
            {
                //File.CreateText(filename);
                ledger = new PasswordLedger();
                File.WriteAllText(filename, JsonConvert.SerializeObject(ledger, Formatting.Indented));
            }
            else
            {
                ledger = JsonConvert.DeserializeObject<PasswordLedger>(File.ReadAllText(filename));
            }
        }

        private void Save()
        {
            // Record pretty-printed JSON to file.
            File.WriteAllText(filename, JsonConvert.SerializeObject(ledger, Formatting.Indented));

            //File.WriteAllText(filename, JsonConvert.SerializeObject(ledger, Formatting.Indented,Q
            //    new JsonConverter[] { new StringEnumConverter() }));
        }

        public string GetPassword(string playerName)
        {
            try
            {
                return ledger.records[playerName];
            }
            catch(KeyNotFoundException)
            {
                return "";
            }
        }

        public string RecordPassword(string playerName, string password)
        {
            if (ledger.records.ContainsKey(playerName))
                ledger.records[playerName] = password;
            else
                ledger.records.Add(playerName, password);
            Save();
            return ledger.records[playerName];
        }

        public void SetStatus(bool isOn)
        {
            if(isOn)
                ledger.status = 1;
            else
                ledger.status = 0;
            Save();
        }

        public bool GetStatus()
        {
            if(ledger.status == 0)
                return false;
            else
                return true;
        }

        public int GetCount(){
            return ledger.records.Count;
        }
        public string GetNameList()
        {
            string s = "";
            foreach (var item in ledger.records)
            {
                s += item.Key + ", ";
            }
            return s;
        }
    }

    class PasswordLedger
    {
        public Dictionary<string, string> records = new Dictionary<string, string>();
        public int status = 1;
    }
}