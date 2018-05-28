using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates {
	private float x;
	private float y;

	public Coordinates() { 
		this.x = 0;
		this.y = 0;
	}

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