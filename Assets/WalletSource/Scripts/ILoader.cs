using GameCurrency;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ILoader
{
    void Load(Action<Dictionary<CurrencyType,int>> action);
    void Save(Dictionary<CurrencyType, int> data);
}