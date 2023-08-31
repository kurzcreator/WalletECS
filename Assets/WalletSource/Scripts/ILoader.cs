using GameCurrency;
using System.Collections.Generic;

public interface ILoader
{
    bool Load(Dictionary<CurrencyType, int> data);
    bool Save(Dictionary<CurrencyType, int> data);
}