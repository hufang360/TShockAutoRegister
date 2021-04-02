using System;
using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace AutoRegister
{
    interface IPasswordRepository
    {
        string RecordPassword(string playerName, string password);
        string GetPassword(string playerName);
        void SetStatus(Boolean isOn);
        Boolean GetStatus();

        int GetCount();
    }

    class JsonPasswordRepository : IPasswordRepository
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

            // Record pretty-printed JSON to file.
            File.WriteAllText(filename, JsonConvert.SerializeObject(ledger, Formatting.Indented));

            //File.WriteAllText(filename, JsonConvert.SerializeObject(ledger, Formatting.Indented,
            //    new JsonConverter[] { new StringEnumConverter() }));

            return ledger.records[playerName];
        }

        public void SetStatus(Boolean isOn)
        {
            if(isOn)
                ledger.status = 1;
            else
                ledger.status = 0;
        }

        public Boolean GetStatus()
        {
            if(ledger.status == 0)
                return false;
            else
                return true;
        }

        public int GetCount(){
            return ledger.records.Count;
        }
    }

    class PasswordLedger
    {
        public Dictionary<string, string> records = new Dictionary<string, string>();
        public int status = 1;
    }
}