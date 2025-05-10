using BallDrop.Base;
using BallDrop.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallDrop
{
    public class ObjectPool : SingletonMonoBehaviour<ObjectPool>
    {
        public Transform PooledObjectsHolder;

        [SerializeField]
        private GameObject Player;
        [SerializeField]
        private GameObject Row;
        [SerializeField]
        private GameObject Coin;
        [SerializeField]
        private GameObject NormalCube;
        [SerializeField]
        private GameObject EnemyCube;
        [SerializeField]
        private GameObject FadedCube;
        [SerializeField]
        private GameObject MovingCube;
        [SerializeField]
        private GameObject XCube;
        [SerializeField]
        private GameObject SpikeCube;
        [SerializeField]
        private GameObject ReverseCube;
        [SerializeField]
        private GameObject InvisibleCube;
        [SerializeField]
        private GameObject LandingCube;
        [SerializeField]
        private GameObject ShieldPowerUp;
        [SerializeField]
        private GameObject SlowDownPowerUp;
        [SerializeField]
        private GameObject ProjectilePrefab;
        [SerializeField]
        private GameObject DynamicScore;
        [SerializeField]
        private GameObject TrailImage;
        [SerializeField]
        private GameObject EffectText;
        [SerializeField]
        private GameObject VerticalBeam;
        [SerializeField]
        private GameObject SplatterEffect;
        [SerializeField]
        private GameObject BreakParticle;
        [SerializeField]
        private GameObject CongratulatoryParticleSystem;
        [SerializeField]
        private GameObject GameOverParticleSystem;
        [SerializeField]
        private GameObject DeathParticleSystem;
        [SerializeField]
        private GameObject CoinCollectionParticles;

        public int PlayerCount = 1;
        public int CoinCount = 10;
        public int RowCount = 10;
        public int NormalCubeCount = 40;
        public int OtherCubeCount = 25;
        public int LandingCubeCount = 1;
        public int ShieldCount = 1;
        public int SlowDownCount = 1;
        public int DynamicScoreCount = 5;
        public int TrailImageCount = 20;
        public int CongratularoyParticleSystemCount = 2;
        public int GameOverParticleSystemCount = 2;
        public int EffectTextCount = 3;
        public int SplatterCount = 5;
        public int BreakParticlesCount = 3;
        public int VerticalBeamCount = 2;
        public int ProjectileCount = 5;
        public int DeathParticleCount = 2;
        public int MaxTweenCount;

        private GameObject CurrentPlayer;
        private GameObject CurrentShield;
        private GameObject CurrentSlowDown;
        private GameObject CurrentCongratulatoryParticleSystem;
        private GameObject CurrentGameOverParticleSystem;


        [HideInInspector]
        public List<GameObject> NormalCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> EnemyCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> MovingCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> SpikeCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> ReverseCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> XCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> FadedCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> InvisibleCubes = new List<GameObject>();
        [HideInInspector]
        public List<GameObject> LandingCubes = new List<GameObject>();

        private List<GameObject> Players = new List<GameObject>();
        private List<GameObject> Rows = new List<GameObject>();
        private List<CoinBase> Coins = new List<CoinBase>();
        private List<GameObject> Shields = new List<GameObject>();
        private List<GameObject> SlowDowns = new List<GameObject>();
        private List<GameObject> DynamicScores = new List<GameObject>();
        private List<TrailImage> TrailImages = new List<TrailImage>();
        private List<ProjectileParticle> projectileParticles = new List<ProjectileParticle>();
        private List<BaseSplatter> SplatterEffects = new List<BaseSplatter>();
        private List<GameObject> CongratulatoryParticleSystems = new List<GameObject>();
        private List<GameObject> GameOverParticleSystems = new List<GameObject>();
        private List<EffectText> EffectTexts = new List<EffectText>();
        private List<BreakParticle> BreakParticles = new List<BreakParticle>();
        public List<GameObject> RandomObjects = new List<GameObject>();
        private List<GameObject> DeathParticles = new List<GameObject>();
        private List<GameObject> CoinCollectionParticleList = new List<GameObject>();
        private List<IPowerup> VerticalBeams = new List<IPowerup>();
        private List<IPowerup> LineGuides = new List<IPowerup>();


        #region Instantiation
        public IEnumerator InstantiateObjects()
        {
            Application.targetFrameRate = 60;

            #region Cubes

            if (NormalCube != null)
            {
                for (int i = 0; i < NormalCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(NormalCube, PooledObjectsHolder);
                    gameObject.name = "Normal Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Normal);
                    NormalCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (EnemyCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(EnemyCube, PooledObjectsHolder);
                    gameObject.name = "Enemy Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Enemy);
                    EnemyCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (FadedCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(FadedCube, PooledObjectsHolder);
                    gameObject.name = "Faded Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Faded);
                    FadedCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (MovingCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(MovingCube, PooledObjectsHolder);
                    gameObject.name = "Moving Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Moving);
                    MovingCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (XCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(XCube, PooledObjectsHolder);
                    gameObject.name = "X Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.X);
                    XCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (SpikeCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(SpikeCube, PooledObjectsHolder);
                    gameObject.name = "Spike Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Spike);
                    SpikeCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (ReverseCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(ReverseCube, PooledObjectsHolder);
                    gameObject.name = "Reverse Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Reverse);
                    ReverseCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (InvisibleCube != null)
            {
                for (int i = 0; i < OtherCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(InvisibleCube, PooledObjectsHolder);
                    gameObject.name = "Invisible Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Invisible);
                    InvisibleCubes.Add(gameObject);
                    yield return null;
                }
            }

            if (LandingCube != null)
            {
                for (int i = 0; i < LandingCubeCount; i++)
                {
                    GameObject gameObject = Instantiate(LandingCube, PooledObjectsHolder);
                    gameObject.name = "Landing Cube - " + i;
                    gameObject.SetActive(false);
                    gameObject.GetComponent<ICube>().SetBaseCubeType(CubeType.Landing);
                    LandingCubes.Add(gameObject);
                    yield return null;
                }
            }

            #endregion Cubes

            #region Player

            if (Player != null)
            {
                for (int i = 0; i < PlayerCount; i++)
                {
                    GameObject gameObject = Instantiate(Player, PooledObjectsHolder);
                    gameObject.name = "Player-" + i;
                    gameObject.SetActive(false);
                    Players.Add(gameObject);
                    yield return null;
                }
            }

            #endregion Player

            #region Row

            if (Row != null)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    GameObject gameObject = Instantiate(Row, PooledObjectsHolder, true);
                    gameObject.name = "Row - " + i;
                    //for (int j = 0; j < gameObject.transform.childCount; j++)
                    //{
                    //    gameObject.transform.GetChild(j).GetComponent<ICube>().Deactivate();
                    //}
                    gameObject.SetActive(false);
                    Rows.Add(gameObject);
                    yield return null;
                }
            }

            #endregion Row

            #region Coin

            if (Coin != null)
            {
                for (int i = 0; i < CoinCount; i++)
                {
                    CoinBase coin = Instantiate(Coin, PooledObjectsHolder, true).GetComponent<CoinBase>();
                    coin.name = "Coin - " + i;
                    coin.SetActive(false);
                    Coins.Add(coin);
                    yield return null;
                }
            }

            #endregion Coin

            #region Powerups

            if (ShieldPowerUp != null)
            {
                for (int i = 0; i < ShieldCount; i++)
                {
                    GameObject gameObject = Instantiate(ShieldPowerUp, PooledObjectsHolder, true);
                    gameObject.name = "Shield";
                    gameObject.SetActive(false);
                    Shields.Add(gameObject);
                    yield return null;
                }
            }

            if (SlowDownPowerUp != null)
            {
                for (int i = 0; i < ShieldCount; i++)
                {
                    GameObject gameObject = Instantiate(SlowDownPowerUp, PooledObjectsHolder, true);
                    gameObject.name = "SlowDown";
                    gameObject.SetActive(false);
                    SlowDowns.Add(gameObject);
                    yield return null;
                }
            }

            if (VerticalBeam != null)
            {
                for (int i = 0; i < VerticalBeamCount; i++)
                {
                    IPowerup powerup = Instantiate(VerticalBeam, PooledObjectsHolder, true).GetComponent<IPowerup>();
                    powerup.name = "Vertical Beam - " + i;
                    powerup.SetActive(false);
                    VerticalBeams.Add(powerup);
                }

            }

            #region Projectile Particles
            if (ProjectilePrefab != null)
            {
                for (int i = 0; i < ProjectileCount; i++)
                {
                    ProjectileParticle projectile = Instantiate(ProjectilePrefab, PooledObjectsHolder, true).GetComponent<ProjectileParticle>();
                    projectile.SetName("Projectile Particle - " + i);
                    projectile.SetActive(false);
                    projectileParticles.Add(projectile);
                    yield return null;
                }

            }
            #endregion Projectile Particles

            #endregion Powerups

            #region Effect Text

            if (EffectText != null)
            {
                for (int i = 0; i < EffectTextCount; i++)
                {
                    EffectText effectText = Instantiate(EffectText, PooledObjectsHolder, true).GetComponent<EffectText>();
                    effectText.name = "Effect Text - " + i;
                    effectText.SetParent(PooledObjectsHolder);
                    effectText.SetActive(false);
                    EffectTexts.Add(effectText);
                    yield return null;
                }
            }

            #endregion Effect Text

            #region Splatter Effect

            if (SplatterEffect != null)
            {
                for (int i = 0; i < SplatterCount; i++)
                {
                    BaseSplatter splatterEffect = Instantiate(SplatterEffect, PooledObjectsHolder, true).GetComponent<BaseSplatter>();
                    splatterEffect.name = "Splatter Effect - " + i;
                    splatterEffect.transform.SetParent(PooledObjectsHolder);
                    splatterEffect.SetActive(false);
                    splatterEffect.UpdateParentObject(PooledObjectsHolder);
                    SplatterEffects.Add(splatterEffect);
                    yield return null;
                }
            }

            #endregion Splatter Effect

            #region Dynamic Score

            if (DynamicScore != null)
            {
                for (int i = 0; i < DynamicScoreCount; i++)
                {
                    GameObject gameObject = Instantiate(DynamicScore, PooledObjectsHolder, true);
                    gameObject.name = "Dynamic Score - " + i;

                    gameObject.SetActive(false);
                    DynamicScores.Add(gameObject);
                    yield return null;
                }

            }

            #endregion Dynamic Score

            #region Trail Image

            if (TrailImage != null)
            {
                for (int i = 0; i < TrailImageCount; i++)
                {
                    GameObject gameObject = Instantiate(TrailImage, PooledObjectsHolder, true);
                    gameObject.name = "Trail Image - " + i;

                    gameObject.SetActive(false);
                    TrailImages.Add(gameObject.GetComponent<TrailImage>());
                    yield return null;
                }

            }

            #endregion Trail Image

            #region Break Particle System

            if (BreakParticle != null)
            {
                for (int i = 0; i < BreakParticlesCount; i++)
                {
                    BreakParticle breakParticle = Instantiate(BreakParticle, PooledObjectsHolder, true).GetComponent<BreakParticle>();
                    breakParticle.name = "Break Particle System - " + i;

                    breakParticle.SetActive(false);
                    BreakParticles.Add(breakParticle);
                    yield return null;
                }


            }

            #endregion Break Particle System

            #region Congratulatory Particle System

            if (CongratulatoryParticleSystem != null)
            {
                for (int i = 0; i < CongratularoyParticleSystemCount; i++)
                {
                    GameObject gameObject = Instantiate(CongratulatoryParticleSystem, PooledObjectsHolder, true);
                    gameObject.name = "Congratularoy Particle System - " + i;

                    gameObject.SetActive(false);
                    CongratulatoryParticleSystems.Add(gameObject);
                    yield return null;
                }

            }

            #endregion Congratulatory Particle System

            #region Game Over Particle System


            if (GameOverParticleSystem != null)
            {
                for (int i = 0; i < GameOverParticleSystemCount; i++)
                {
                    GameObject gameObject = Instantiate(GameOverParticleSystem, PooledObjectsHolder, true);
                    gameObject.name = "Game Over Particle System - " + i;

                    gameObject.SetActive(false);
                    GameOverParticleSystems.Add(gameObject);
                    yield return null;
                }

            }

            #endregion Game Over Particle System

            #region Death Particles
            if (DeathParticleSystem != null)
            {
                for (int i = 0; i < DeathParticleCount; i++)
                {
                    GameObject gameObject = Instantiate(DeathParticleSystem, PooledObjectsHolder, true);
                    gameObject.name = "Death Particle System - " + i;
                    gameObject.SetActive(false);
                    DeathParticles.Add(gameObject);
                    yield return null;
                }
            }
            #endregion

            #region Coin Collection Effect Particles
            if (CoinCollectionParticles != null)
            {
                for (int i = 0; i < DeathParticleCount; i++)
                {
                    GameObject gameObject = Instantiate(CoinCollectionParticles, PooledObjectsHolder, true);
                    gameObject.name = "Coin Collection Particle System - " + i;
                    //gameObject.SetActive(false);
                    CoinCollectionParticleList.Add(gameObject);
                    yield return null;
                }
            }
            #endregion

            MyEventManager.OnObjectsInstantiated.Dispatch();
        }
        #endregion

        #region GetObjects
        public GameObject GetPlayer()
        {
            if (CurrentPlayer == null)
            {
                foreach (GameObject player in Players)
                //for (int i = 0; i < Players.Count; i++)
                {
                    //GameObject player = Players.Pop();
                    if (!player.activeInHierarchy)
                    {
                        CurrentPlayer = player;
                        break;
                    }
                }
            }
            return CurrentPlayer;
        }

        public GameObject GetDeathParticle()
        {
            foreach (GameObject go in DeathParticles)
            {
                if (!go.activeInHierarchy)
                    return go;
            }
            GameObject gameObject = Instantiate(DeathParticleSystem, PooledObjectsHolder);
            gameObject.SetActive(false);
            DeathParticles.Add(gameObject);
            return gameObject;
        }

        public GameObject GetRow()
        {
            foreach (GameObject Row in Rows)
            {
                if (!Row.activeInHierarchy)
                {
                    return Row;
                }
            }

            GameObject gameObject = Instantiate(Row, PooledObjectsHolder, true);
            gameObject.name = "Row - Expanded";
            gameObject.SetActive(false);
            Rows.Add(gameObject);
            return gameObject;
        }

        public EffectText GetEffectText()
        {
            foreach (EffectText effectText in EffectTexts)
            {
                if (!effectText.activeInHierarchy)
                {
                    return effectText;
                }
            }

            EffectText effect = Instantiate(EffectText, PooledObjectsHolder, true).GetComponent<EffectText>();
            effect.name = "Effect Text - Expanded";
            effect.SetActive(false);
            EffectTexts.Add(effect);
            return effect;

        }

        public BaseSplatter GetSplatterEffect()
        {
            foreach (BaseSplatter splatterEffect in SplatterEffects)
            {
                if (!splatterEffect.activeInHierarchy)
                {
                    return splatterEffect;
                }
            }

            BaseSplatter splatter = Instantiate(SplatterEffect, PooledObjectsHolder, true).GetComponent<BaseSplatter>();
            splatter.name = "SplatterEffect - Expanded";
            splatter.SetActive(false);
            SplatterEffects.Add(splatter);
            return splatter;
        }

        public CoinBase GetCoin()
        {
            foreach (CoinBase c in Coins)
            {
                if (!c.ActiveInHierarchy)
                {
                    return c;
                }
            }

            CoinBase coin = Instantiate(Coin, PooledObjectsHolder, true).GetComponent<CoinBase>();
            coin.name = "Coin - Expanded";
            coin.SetActive(false);
            Coins.Add(coin);
            return coin;
        }

        public BreakParticle GetBreakParticle()
        {
            foreach (BreakParticle breakParticle in BreakParticles)
            {
                if (!breakParticle.activeInHierarchy)
                {
                    return breakParticle;
                }
            }

            BreakParticle particle = Instantiate(BreakParticle, PooledObjectsHolder, true).GetComponent<BreakParticle>();
            particle.name = "Break Particle - Expanded";
            particle.SetActive(false);
            BreakParticles.Add(particle);
            return particle;
        }

        public TrailImage GetTrailImage()
        {
            foreach (TrailImage trailImage in TrailImages)
            {
                if (!trailImage.gameObject.activeInHierarchy)
                {
                    return trailImage;
                }
            }

            GameObject gameObject = Instantiate(TrailImage, PooledObjectsHolder, true);
            gameObject.name = "Trail Image - Expanded";
            gameObject.SetActive(false);
            TrailImages.Add(gameObject.GetComponent<TrailImage>());
            return gameObject.GetComponent<TrailImage>();
        }

        public GameObject GetRandomObject()
        {
            foreach (GameObject go in RandomObjects)
            {
                if (!go.activeInHierarchy)
                {
                    return go;
                }
            }
            return null;

        }

        public GameObject GetCube(CubeType cubeType)
        {
            List<GameObject> cubes;
            GameObject Cube;
            switch (cubeType)
            {
                case CubeType.Normal:
                    Cube = NormalCube;
                    cubes = NormalCubes;
                    break;

                case CubeType.Enemy:
                    Cube = EnemyCube;
                    cubes = EnemyCubes;
                    break;

                case CubeType.Faded:
                    Cube = FadedCube;
                    cubes = FadedCubes;
                    break;

                case CubeType.Moving:
                    Cube = MovingCube;
                    cubes = MovingCubes;
                    break;

                case CubeType.X:
                    Cube = XCube;
                    cubes = XCubes;
                    break;

                case CubeType.Spike:
                    Cube = SpikeCube;
                    cubes = SpikeCubes;
                    break;
                case CubeType.Reverse:
                    Cube = ReverseCube;
                    cubes = ReverseCubes;
                    break;

                case CubeType.Invisible:
                    Cube = InvisibleCube;
                    cubes = InvisibleCubes;
                    break;

                case CubeType.Landing:
                    Cube = LandingCube;
                    cubes = LandingCubes;
                    break;

                default:
                    Cube = NormalCube;
                    cubes = NormalCubes;
                    break;
            }
            foreach (GameObject cube in cubes)
            {
                if (!cube.activeInHierarchy)
                {
                    return cube;
                }
            }

            GameObject gameObject = Instantiate(Cube, PooledObjectsHolder, true);
            gameObject.name = "Cube Expanded -" + cubeType;
            gameObject.SetActive(false);
            cubes.Add(gameObject);
            return gameObject;
        }

        public GameObject GetCongratulatoryParticleSystem()
        {
            if (CurrentCongratulatoryParticleSystem == null)
            {
                foreach (GameObject congratulatoryPS in CongratulatoryParticleSystems)
                //for (int i = 0; i < Players.Count; i++)
                {
                    //GameObject player = Players.Pop();
                    if (!congratulatoryPS.activeInHierarchy)
                    {
                        CurrentCongratulatoryParticleSystem = congratulatoryPS;
                        break;
                    }
                }
            }
            return CurrentCongratulatoryParticleSystem;
        }

        public GameObject GetGameOverParticleSystem()
        {
            if (CurrentGameOverParticleSystem == null)
            {
                foreach (GameObject gameoverps in GameOverParticleSystems)
                //for (int i = 0; i < Players.Count; i++)
                {
                    //GameObject player = Players.Pop();
                    if (!gameoverps.activeInHierarchy)
                    {
                        CurrentGameOverParticleSystem = gameoverps;
                        break;
                    }
                }
            }
            return CurrentGameOverParticleSystem;
        }

        public GameObject GetCoinCollectionParticles()
        {
            foreach (GameObject ccp in CoinCollectionParticleList)
            {
                if (!ccp.GetComponent<ParticleSystem>().isPlaying)
                {
                    return ccp;
                }
            }

            GameObject particle = Instantiate(CoinCollectionParticles, PooledObjectsHolder, true);
            particle.name = "CCP - Expanded";
            //particle.SetActive(false);
            CoinCollectionParticleList.Add(particle);
            return particle;
        }

        public GameObject GetDynamicScore()
        {
            foreach (GameObject dynamicScore in DynamicScores)
            {
                if (!dynamicScore.activeInHierarchy)
                {
                    return dynamicScore;
                }
            }

            GameObject gameObject = Instantiate(DynamicScore, PooledObjectsHolder, true);
            gameObject.name = "Dynamic Score - Expanded";
            gameObject.SetActive(false);
            DynamicScores.Add(gameObject);
            return gameObject;
        }

        public GameObject GetShieldPowerUp()
        {
            foreach (GameObject shield in Shields)
            {
                if (!shield.activeInHierarchy)
                {
                    return shield;
                }
            }

            GameObject gameObject = Instantiate(ShieldPowerUp, PooledObjectsHolder, true);
            gameObject.name = "Shield - Expanded";
            gameObject.SetActive(false);
            Shields.Add(gameObject);
            return gameObject;
        }

        public GameObject GetSlowDownPowerUp()
        {
            foreach (GameObject slowdown in SlowDowns)
            {
                if (!slowdown.activeInHierarchy)
                {
                    return slowdown;
                }
            }

            GameObject gameObject = Instantiate(SlowDownPowerUp, PooledObjectsHolder, true);
            gameObject.name = "Slowdown - Expanded";
            gameObject.SetActive(false);
            SlowDowns.Add(gameObject);
            return gameObject;
        }

        public IPowerup GetVerticalBeamPowerup()
        {
            foreach (IPowerup beam in VerticalBeams)
            {
                if (!beam.activeInHierarchy)
                {
                    return beam;
                }
            }

            IPowerup verticalBeam = Instantiate(VerticalBeam, PooledObjectsHolder, true).GetComponent<IPowerup>();
            verticalBeam.name = "Vertical Beam - Expanded";
            verticalBeam.SetActive(false);
            VerticalBeams.Add(verticalBeam);
            return verticalBeam;
        }

        public ProjectileParticle GetProjectile()
        {
            foreach (ProjectileParticle projectile in projectileParticles)
            {
                if (!projectile.activeInHierarchy)
                {
                    return projectile;
                }
            }

            ProjectileParticle projectileParticle = Instantiate(ProjectilePrefab, PooledObjectsHolder, true).GetComponent<ProjectileParticle>();
            projectileParticle.name = "Projectile - Expanded";
            projectileParticle.SetActive(false);
            projectileParticles.Add(projectileParticle);
            return projectileParticle;
        }

        //public IPowerup GetLineGuidePowerup()
        //{
        //    foreach (IPowerup lineGuide in LineGuides)
        //    {
        //        if (!lineGuide.activeInHierarchy)
        //        {
        //            return lineGuide;
        //        }
        //    }

        //    IPowerup guide = Instantiate(LineGuidePrefab, PooledObjectsHolder, true).GetComponent<IPowerup>();
        //    guide.name = "LineGuide - Expanded";
        //    guide.SetActive(false);
        //    LineGuides.Add(guide);
        //    return guide;
        //}

            #endregion

        #region Others
        public int GetTotalObjectCount()
        {
            int Total = 0;
            if (Player != null)
                Total += PlayerCount;
            if (Coin != null)
                Total += CoinCount;
            if (Row != null)
                Total += RowCount;
            if (NormalCube != null)
                Total += NormalCubeCount;
            if (EnemyCube != null)
                Total += OtherCubeCount;
            if (MovingCube != null)
                Total += OtherCubeCount;
            if (FadedCube != null)
                Total += OtherCubeCount;
            if (XCube != null)
                Total += OtherCubeCount;
            if (SpikeCube != null)
                Total += OtherCubeCount;
            if (InvisibleCube != null)
                Total += OtherCubeCount;
            if (LandingCube != null)
                Total += LandingCubeCount;
            if (ShieldPowerUp != null)
                Total += ShieldCount;
            if (SlowDownPowerUp != null)
                Total += SlowDownCount;
            if (DynamicScore != null)
                Total += DynamicScoreCount;
            if (TrailImage != null)
                Total += TrailImageCount;
            if (CongratulatoryParticleSystems != null)
                Total += CongratularoyParticleSystemCount;
            if (GameOverParticleSystem != null)
                Total += GameOverParticleSystemCount;
            if (EffectText != null)
                Total += EffectTextCount;
            if (VerticalBeam != null)
                Total += VerticalBeamCount * 2;
            if (SplatterEffect != null)
                Total += SplatterCount;
            if (BreakParticle != null)
                Total += BreakParticlesCount;
            if (ProjectilePrefab != null)
                Total += ProjectileCount;
            return Total;
        }

        public void DeactivateLeftCubes()
        {
            Deactivate(NormalCubes);
            Deactivate(EnemyCubes);
            Deactivate(FadedCubes);
            Deactivate(MovingCubes);
            Deactivate(XCubes);
            Deactivate(SpikeCubes);
            Deactivate(ReverseCubes);
            Deactivate(InvisibleCubes);
            Deactivate(LandingCubes);
        }

        private void Deactivate(List<GameObject> cubelist)
        {
            foreach (GameObject cube in cubelist)
            {
                if (cube.activeInHierarchy)
                {
                    cube.GetComponent<ICube>().Deactivate();
                    cube.transform.SetParent(PooledObjectsHolder);
                }
            }
        }

        public List<TrailImage> GetTrailsList()
        {
            return TrailImages;
        }

        public void SetTrails()
        {
            foreach (TrailImage trailImage in TrailImages)
            {
                trailImage.GetComponent<Billboard>().SetCamera();
            }

        }
        public void DeactivateTrails()
        {
            foreach (TrailImage trail in TrailImages)
            {
                if (trail.gameObject.activeInHierarchy)
                    trail.Deactivate();
            }
        }
        #endregion
    }
}