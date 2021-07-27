using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;
using EntityStates;
using EntityStates.Commando;	
using EntityStates.Merc;

namespace V1Mod.SkillStates
{
    // Token: 0x02000BC8 RID: 3016
    public class Slide2 : BaseSkillState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            Util.PlaySound(SlideState.soundString, base.gameObject);
			if (base.inputBank && base.characterDirection)
			{
			}
			if (base.characterMotor)
			{
				this.startedStateGrounded = base.characterMotor.isGrounded;
			}
		}
		public override void FixedUpdate()
		{
			base.FixedUpdate();
			bool flag2 = !this.buttonReleased && base.inputBank && !base.inputBank.skill3.down;
			if (flag2)
			{
				this.buttonReleased = true;
			}
			if (base.isAuthority)
			{
				Util.PlaySound("RevolverShot", base.gameObject);
				Vector3 velocity = base.characterMotor.velocity;
				velocity.x = 12;
				base.characterMotor.velocity = velocity;
			}
			bool flag4 = !base.inputBank || (!base.inputBank.skill3.down);
			if (flag4)
			{
				this.outer.SetNextStateToMain();
				return;
			}
		}	
		private bool startedStateGrounded;
		private bool buttonReleased = false;
	}
}

