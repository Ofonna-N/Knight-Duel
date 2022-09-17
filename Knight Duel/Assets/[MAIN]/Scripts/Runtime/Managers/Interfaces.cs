using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KnightDuel
{
    interface IRecieveCharacterAnimEvents
    {
        public void IOnActivateWeapon();

        public void IOnDeactivateWeapon();

        public void IOCharacterDodge();
    }

    interface ISDK_Integration
    {
        public void IAwake();

        public void IStart();

    }
}
