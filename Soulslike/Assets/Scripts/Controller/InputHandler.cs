﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA {
	public class InputHandler : MonoBehaviour {

		float vertical, horizontal;
		bool a_input, b_input, x_input, y_input, rb_input, rt_input, lb_input, lt_input;
		float rt_axis, lt_axis;

		StateManager states;
		CameraManager camManager;

		float delta;

		void Start () {
			states = GetComponent<StateManager> ();
			states.Init ();

			camManager = CameraManager.singleton;
			camManager.Init (this.transform);
		}

		void Update () {
			delta = Time.deltaTime;
			states.Tick (delta);
		}

		void FixedUpdate () {
			delta = Time.fixedDeltaTime;
			GetInput ();
			UpdateStates ();
			states.FixedTick (delta);
			camManager.Tick (delta);
		}

		void GetInput () {
			vertical = Input.GetAxis ("Vertical");
			horizontal = Input.GetAxis ("Horizontal");
			a_input = Input.GetButton ("a_input");
			b_input = Input.GetButton ("b_input");
			x_input = Input.GetButton ("x_input");
			y_input = Input.GetButtonUp ("y_input");
			rt_input = Input.GetButton ("rt_input");
			rt_axis = Input.GetAxis ("rt_input");
			if (rt_axis != 0)
				rt_input = true;
			lt_input = Input.GetButton ("lt_input");
			lt_axis = Input.GetAxis ("lt_input");
			if (lt_axis != 0)
				lt_input = true;
			rb_input = Input.GetButton ("rb_input");
			lb_input = Input.GetButton ("lb_input");
		}

		void UpdateStates () {
			states.vertical = vertical;
			states.horizontal = horizontal;

			Vector3 v = vertical * camManager.transform.forward;
			Vector3 h = horizontal * camManager.transform.right;
			states.moveDir = (v + h).normalized;
			float m = Mathf.Abs (horizontal) + Mathf.Abs (vertical);
			states.moveAmount = Mathf.Clamp01 (m);

			if (b_input) {
				states.isRunning = states.moveAmount > 0;
			} else {
				states.isRunning = false;
			}

			states.rb = rb_input;
			states.rt = rt_input;
			states.lb = lb_input;
			states.lt = lt_input;

			if (y_input) {
				states.isTwoHanded = !states.isTwoHanded;
				states.HandleTwoHanded ();
			}
		}
	}
}