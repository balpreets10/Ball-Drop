//using System;
//using System.Collections;
//using UnityEngine;

//namespace BallDrop
//{
//    public class MenuManager : SingletonMonoBehaviour<MenuManager>
//    {
//        public Menu CurrentMenu = null;
//        public Menu PreviousMenu = null;

//        public Menu MenuSplash;
//        public Menu MenuMain;
//        public Menu MenuGame;
//        public Menu MenuLevelEnd;

//        public void Start()
//        {
//            ShowMenu(MenuType.Splash, 0);
//        }

//        private void Update()
//        {
//            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Backspace))
//            {
//                if (CurrentMenu.myMenuType == MenuType.Game)
//                {
//                    MyEventManager.Instance.PauseGame.Dispatch();
//                }
//            }
//        }

//        private void OnEnable()
//        {
//            MyEventManager.Instance.ShowMenu.AddListener(ShowMenu);
//        }

//        private void OnDisable()
//        {
//            if (MyEventManager.Instance != null)
//            {
//                MyEventManager.Instance.ShowMenu.RemoveListener(ShowMenu);
//            }
//        }

//        public void ShowMenu(MenuType menuType, float delay)
//        {
//            StartCoroutine(ShowMenuWithDelay(menuType, delay));
//        }

//        private IEnumerator ShowMenuWithDelay(MenuType menuType, float delay)
//        {
//            PreviousMenu = CurrentMenu;
//            yield return new WaitForSeconds(delay);
//            PreviousMenu.HideMenu();
//            switch (menuType)
//            {
//                case MenuType.Splash:
//                    CurrentMenu = MenuSplash;
//                    break;
//                case MenuType.Main:
//                    CurrentMenu = MenuMain;
//                    break;
//                case MenuType.Game:
//                    CurrentMenu = MenuGame;
//                    break;
//                case MenuType.End:
//                    CurrentMenu = MenuLevelEnd;
//                    break;
//            }
//            CameraController.Instance.SetInitialPosition();
//            CurrentMenu.ShowMenu();
//        }

//        private void HideMenu(Menu menuToHide)
//        {
//            menuToHide.HideMenu();
//        }


//    }

//    public enum MenuType
//    {
//        Splash,
//        Main,
//        Game,
//        End
//    }
//}