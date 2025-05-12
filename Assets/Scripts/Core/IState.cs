using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState<T> where T : IConvertible
{
    T GetState();
}