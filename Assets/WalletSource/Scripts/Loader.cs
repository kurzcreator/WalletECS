using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Unity.Serialization.Json;
using UnityEngine;

namespace GameCurrency
{
    public class PlayerPrefsLoader : ILoader
    {
        public void Load(Action<Dictionary<CurrencyType, int>> action)
        {
            Dictionary<CurrencyType, int> currencyDictionary = new Dictionary<CurrencyType, int>();

            var goldKey = GameConstants.PlayerPrefsKeys.GOLD_KEY;
            if (PlayerPrefs.HasKey(goldKey))
            {
                currencyDictionary.Add(CurrencyType.Gold, PlayerPrefs.GetInt(goldKey));
            }

            var crystalKey = GameConstants.PlayerPrefsKeys.CRYSTAL_KEY;
            if (PlayerPrefs.HasKey (crystalKey))
            {
                currencyDictionary.Add(CurrencyType.Crystal, PlayerPrefs.GetInt(crystalKey));
            }

            if (currencyDictionary.Count == 0)
            {
                action?.Invoke(null);
            }

            action?.Invoke(currencyDictionary);
        }
     
        public void Save(Dictionary<CurrencyType, int> data)
        {
            foreach (var item in data)
            {
                switch (item.Key) 
                {
                    case CurrencyType.Crystal:
                        PlayerPrefs.SetInt(GameConstants.PlayerPrefsKeys.CRYSTAL_KEY, item.Value); 
                        break;

                    case CurrencyType.Gold:
                        PlayerPrefs.SetInt(GameConstants.PlayerPrefsKeys.GOLD_KEY, item.Value);
                        break;

                    case CurrencyType.None:
                        break;
                }
            }
        }
    }

    public class FileLoader : ILoader
    {
        public const string FileName = "/Wallet.json";
        public string Path => Application.persistentDataPath + FileName;

        public async void Load(Action<Dictionary<CurrencyType, int>> action)
        {
            using (StreamReader stream = new StreamReader(Path)) 
            {
                string json = await stream.ReadToEndAsync();
                Dictionary<CurrencyType, int> currencyDictionary = null;

                try
                {
                    currencyDictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<CurrencyType, int>>(json);
                }
                catch (Exception ex) 
                {
                    throw new Exception($"File read error {ex.Message}");
                }

                action?.Invoke(currencyDictionary);
            }
        }

        public async void Save(Dictionary<CurrencyType, int> data)
        {

            using (StreamWriter stream = new StreamWriter(Path)) 
            {
                try
                {
                    var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                    await stream.WriteAsync(json);

                }
                catch (Exception ex)
                {
                    throw new Exception($"Failed to save file {Path} {ex.Message}");
                }
            }
        }
    }
}