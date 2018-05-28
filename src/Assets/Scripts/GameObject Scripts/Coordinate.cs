﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour {
	private float x;
	private float y;

    public Coordinates(float x, float y)
	{
		this.x = x;
		this.y = y;
	}
    
	public float getX(){
		return this.x;
	}

	public float getY(){
		return this.y;
	}

	public void setXY(float x, float y){
		this.x = x;
		this.y = y;
	}
}