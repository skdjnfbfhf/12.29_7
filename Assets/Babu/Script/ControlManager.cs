using System.Collections;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Bson;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Babu
{
    public class ControlManager : MonoBehaviour
    {
        public class MouseRect
        {
            public int left;
            public int right;
            public int top;
            public int bottom;
        }
        public class CameraPos
        {
            public float left = 5;
            public float right = 25;
            public float top = 20;
            public float bottom = 0;
        }
        public MouseRect mouseRect = new MouseRect();
        public int moveInterval = 20;
        CameraPos cameraPos = new CameraPos();
        public float cameraSpeed = 1.0f;
        Camera mainCam;
        public MapManager mapManager;
        public int buildingSize = 0;
        public GameObject currentTower;
        public GameObject NomalTower;
        //public TowerName towerName;
        public int ErrorNum = -1;

        

        void Start()
        {
            Cursor.lockState = CursorLockMode.Confined;
            mapManager = this.GetComponent<MapManager>();
            mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
            mouseRect.left = moveInterval;
            mouseRect.bottom = moveInterval;
            mouseRect.right = Screen.width - 1 - moveInterval;
            mouseRect.top = Screen.height - 1 - moveInterval;
        }

        // Update is called once per frame
        void Update()
        {
            MouseControl();
            CreateDefenceTower();
        }

        public void CreateDefenceTower()
        {
            int posX = -1;
            int posZ = -1;
            if(currentTower != null)
            {
                if(buildingSize > 0)
                {
                    
                    Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                    if(Physics.Raycast(ray, out RaycastHit raycastHit))
                    {
                        posX = Mathf.FloorToInt(raycastHit.point.x);
                        posZ = Mathf.FloorToInt(raycastHit.point.z);
                        bool isBuilding = mapManager.isRoad(posX, posZ, buildingSize);
                        if(isBuilding)
                        {
                            currentTower.SetActive(true);
                            currentTower.transform.position = new Vector3(posX, 0.2f, posZ);
                            ErrorNum = -1;
                        }
                        else
                        {
                            currentTower.SetActive(false);
                            ErrorNum = 0;
                        }
                    }
                    else
                    {
                        ErrorNum = 0;
                    }
                }
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        if(currentTower != null)
                        {
                            if (ErrorNum != -1)
                            {
                            }
                            else
                            {
                                mapManager.ChangeBuild((int)currentTower.transform.position.x, (int)currentTower.transform.position.z, buildingSize, BlockName.Build);
                                currentTower = null;
                            }
                        }
                    }
                }

            }
        }
        public void MouseControl()
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                Cursor.lockState = CursorLockMode.None;
            }
            Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            if(mouseRect.left > mousePos.x)
            {
                if(cameraPos.left < mainCam.transform.position.x)
                {
                    mainCam.transform.position -= new Vector3(1.0f, 0, 0) * 1.0f * Time.deltaTime;
                }
            }
            if(mouseRect.right < mousePos.x)
            {
                if(cameraPos.right > mainCam.transform.position.x)
                {
                    mainCam.transform.position += new Vector3(1.0f, 0, 0) * 1.0f * Time.deltaTime;

                }
            }
            if (mouseRect.top < mousePos.y)
            {
                if (cameraPos.top > mainCam.transform.position.z)
                {
                    mainCam.transform.position += new Vector3(0, 0, 1.0f) * 1.0f * Time.deltaTime;
                }
            }
            if (mouseRect.bottom > mousePos.y)
            {
                if (cameraPos.bottom < mainCam.transform.position.z)
                {
                    mainCam.transform.position -= new Vector3(0, 0, 1.0f) * 1.0f * Time.deltaTime;
                }
            }
        }

        public void CreateTower()
        {
            currentTower = Instantiate(NomalTower);
        }

        public void BtnBuild()
        {
            CreateTower();
            buildingSize = 1;
        }
    }
}
