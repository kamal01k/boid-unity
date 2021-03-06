﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{

    public Vector3 velocity;

    private Vector3 maxVelocity = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 maxAcceleration = new Vector3(0.0075f, 0.0075f, 0.0015f);
    private Vector3 initialPosition;
    private float maxDistance = 3.0f;
    private Boid[] population;

    public void Initialise(Boid[] population)
    {
        this.population = population;
    }

    // Use this for initialization
    void Start()
    {
        velocity = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        velocity = Vector3.Scale(velocity.normalized, maxVelocity);
        initialPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }
	
    // Update is called once per frame
    void Update()
    {
        var acceleration = Vector3.zero;
        var distanceFromOrigin = Vector3.Distance(transform.position, initialPosition);
        if (distanceFromOrigin > maxDistance)
        {
            acceleration += moveTo(initialPosition);
        }
        acceleration += alignWithNeighbours();
        acceleration = Vector3.Scale(acceleration.normalized, maxAcceleration);
        velocity += acceleration;
        velocity = max(velocity, maxVelocity);
        transform.position += velocity;
        transform.rotation = Quaternion.FromToRotation(Vector3.up, velocity);
    }

    private Vector3 moveTo(Vector3 target)
    {
        var acceleration = new Vector3(target.x, target.y, target.z);
        acceleration -= transform.position;
        return acceleration;
    }

    private Vector3 alignWithNeighbours()
    {
        var acceleration = Vector3.zero;
        for (var i = 0; i < population.Length; i++)
        {
            acceleration += population[i].velocity;
        }
        return acceleration.normalized;
    }

    private Vector3 max(Vector3 v1, Vector3 v2)
    {
        var maxVector = new Vector3(v1.x, v1.y, v1.z);
        maxVector.x = Mathf.Max(maxVector.x, -v2.x);
        maxVector.y = Mathf.Max(maxVector.y, -v2.y);
        maxVector.z = Mathf.Max(maxVector.z, -v2.z);
        maxVector.x = Mathf.Min(maxVector.x, v2.x);
        maxVector.y = Mathf.Min(maxVector.y, v2.y);
        maxVector.z = Mathf.Min(maxVector.z, v2.z);
        return maxVector;
    }
}