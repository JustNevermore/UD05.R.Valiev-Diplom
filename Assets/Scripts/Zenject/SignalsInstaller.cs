﻿using Signals;

namespace Zenject
{
    public class SignalsInstaller : Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<OnChangeWeaponTypeSignal>();
            Container.DeclareSignal<OnWeaponBehawiourChange>();
        }
    }
}