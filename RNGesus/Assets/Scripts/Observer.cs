using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Observer
{
    void updateFromSubject();
    void updateFromSubject(object o);
}
