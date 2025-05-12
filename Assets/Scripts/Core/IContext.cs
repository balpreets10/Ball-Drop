using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContext<T> where T : IConvertible
{
    T GetContext();

    T GetLastContext();
}