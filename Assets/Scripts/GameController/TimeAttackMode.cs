﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeAttackMode : GameMode
{
    Car carPlayer1;
    float timeLeft;
    float totalTime;

    public override void Activate()
    {
        //Generate Player1 car
        carPlayer1 = Instantiate(gameController.player1Prefab, gameController.startPoints[0].position, gameController.startPoints[0].rotation).GetComponent<Car>();

        //Add Car to Game Controller
        gameController.cars.Add(carPlayer1);
        carPlayer1.SetGameController(gameController);
        
        //Init camera
        Camera.main.gameObject.GetComponent<CameraFollow>().target = carPlayer1.transform;

        //Init mode variables
        timeLeft = gameController.timeLimit;
        totalTime = 0;

        //Init UI
        menuUI.DeactivateMenu();
        gameUI.ActivateTimeUI();
        gameUI.SetTimeLeftTime(timeLeft);
        gameUI.UpdateP1Speed(carPlayer1.ForwardVelocity().magnitude, carPlayer1.maxSpeed);

        //Count down starts
        StartCoroutine(gameController.CountDown(3));
    }

    void LateUpdate()
    {
        timeLeft -= Time.deltaTime;
        totalTime += Time.deltaTime;

        //Update UI
        gameUI.SetTimeLeftTime(timeLeft);
        gameUI.UpdateP1Speed(carPlayer1.ForwardVelocity().magnitude, carPlayer1.maxSpeed);

        //Check gameover
        if (timeLeft <= 0)
        {
            gameController.GameOver();
            gameUI.SetResultText("You lasted " + Mathf.FloorToInt(totalTime) + " seconds");
            timeLeft = 0;
            gameUI.SetTimeLeftTime(timeLeft);
        }
    }
}