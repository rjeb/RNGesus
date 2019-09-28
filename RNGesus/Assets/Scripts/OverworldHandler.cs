using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverworldHandler : MonoBehaviour
{

    private class MapTree
    {
        private MapNode firstLevel;

        public MapTree()
        {
            firstLevel = new MapNode();
        }

        public MapNode newMap(GameObject button, Vector3 position, MapNode pred)
        {
            newMap = new MapNode(button, position, pred);
        }
        
        private class MapNode
        {
            private GameObject sprite;
            private Vector3 position;
            private bool discovered, accessible;
            private MapNode predecessor;

            private MapNode()
            {
                position = new Vector3(0, 0, 0);
                discovered = true;
                reachable - true;
            }

            private MapNode(GameObject sprite, Vector3 position, MapNode pred)
            {
                discovered = false;
                reachable = false;
            }

            private MapNode getNext()
            {
                return nextNode;
            }

            private void setNext(MapNode next)
            {
                nextNode = next;
            }

            private bool discovered()
            {
                return discovered;
            }

            private bool access()
            {
                return accessible;
            }

            private void makeAvailable()
            {
                discovered = true;
                accessible = false;
            }

            private void clear()
            {
                accessible = false;
            }
        }
    }



    public GameObject mapNode;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(mapNode, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
