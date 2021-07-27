using System;
using RoR2;
using UnityEngine;
using UnityEngine.Networking;

namespace V1Mod.Modules.Misc
{
	// Token: 0x0200002C RID: 44
	[RequireComponent(typeof(CharacterBody))]
	internal class V1PassiveController : NetworkBehaviour
	{
		// Token: 0x060000FF RID: 255 RVA: 0x0001045F File Offset: 0x0000E65F
		private void Awake()
		{
			this.characterBody = base.GetComponent<CharacterBody>();
		}

		// Token: 0x040001D0 RID: 464
		private CharacterBody characterBody;

		// Token: 0x040001D1 RID: 465
		public GenericSkill passiveSkill;
	}
}