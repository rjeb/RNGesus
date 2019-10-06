using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Subject
{
    void registerObserver(Observer o);
    void unregisterObserver(Observer o);
    void notifyObservers();
    void notifyObservers(object o);
}
