using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataService
{
    bool SaveData<T>(string RelativePath, T Data, bool Encrypted);

    T LoadData<T>(string RelativePath, bool Encrypted);
}
