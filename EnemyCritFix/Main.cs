using BepInEx;
using BepInEx.Configuration;
using EntityStates;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using R2API;
using RoR2;
using System;
using UnityEngine;

namespace EnemyCritFix
{
    // Dependencies
    [BepInDependency(RecalculateStatsAPI.PluginGUID)]

    // Metadata
    [BepInPlugin("Samuel17.EnemyCritFix", "EnemyCritFix", "1.0.1")]

    public class Main : BaseUnityPlugin
    {
        // Config fields
        public static ConfigEntry<float> enemyCritChance { get; private set; }

        public void Awake()
        {
            // Logging!
            Log.Init(Logger);

            // Configs
            enemyCritChance = Config.Bind("heha", "Monster Crit Chance", 0f, "Bonus crit percentage applied to all enemies. Set to a negative value (or zero) to disable.");

            // Mod logic
            CritHooks();

            if (enemyCritChance.Value > 0)
            {
                RecalculateStatsAPI.GetStatCoefficients += EnemyCritChance;
            }
        }

        private void CritHooks()
        {
            // Beetle
            On.EntityStates.BeetleMonster.HeadbuttState.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.attack != null)
                {
                    self.attack.isCrit = self.RollCrit();
                }
            };

            // Jellyfish
            IL.EntityStates.JellyfishMonster.JellyNova.Detonate += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.JellyfishMonster.JellyNova>(OpCodes.Ldfld, nameof(EntityStates.JellyfishMonster.JellyNova.attack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Jellyfish crit hook failed!");
                }
            };

            // Beetle Guard slam
            On.EntityStates.BeetleGuardMonster.GroundSlam.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.attack != null)
                {
                    self.attack.isCrit = self.RollCrit();
                }
            };

            // Bighorn Bison heabutt
            On.EntityStates.Bison.Headbutt.OnEnter += (orig, self) =>
            {
                orig(self);
                if (self.attack != null)
                {
                    self.attack.isCrit = self.RollCrit();
                }
            };

            // Bighorn Bison charge
            On.EntityStates.Bison.Charge.ResetOverlapAttack += (orig, self) =>
            {
                orig(self);
                if (self.attack != null)
                {
                    self.attack.isCrit = self.RollCrit();
                }
            };

            // Lynx Berserker slam
            IL.EntityStates.Lynx2Monster.FrenziedUpsetPoundSlamState.DetonateAuthority += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.Lynx2Monster.FrenziedUpsetPoundSlamState>(OpCodes.Ldfld, nameof(EntityStates.Lynx2Monster.FrenziedUpsetPoundSlamState.attack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Lynx Berserker slam crit hook failed!");
                }
            };

            // Stone Golem laser
            IL.EntityStates.GolemMonster.FireLaser.OnEnter += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Dup);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Stone Golem laser crit hook failed!");
                }
            };

            // Stone Golem clap
            IL.EntityStates.GolemMonster.ClapState.FixedUpdate += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.GolemMonster.ClapState>(OpCodes.Ldfld, nameof(EntityStates.GolemMonster.ClapState.attack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Stone Golem clap crit hook failed!");
                }
            };

            // Parent
            IL.EntityStates.ParentMonster.GroundSlam.FixedUpdate += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.ParentMonster.GroundSlam>(OpCodes.Ldfld, nameof(EntityStates.ParentMonster.GroundSlam.attack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Parent crit hook failed!");
                }
            };

            // Clay Apothecary slam (melee)
            IL.EntityStates.ClayGrenadier.FaceSlam.FixedUpdate += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.ClayGrenadier.FaceSlam>(OpCodes.Ldfld, nameof(EntityStates.ClayGrenadier.FaceSlam.attack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Clay Apothecary melee slam crit hook failed!");
                }
            };

            // Halcyonite laser
            IL.EntityStates.Halcyonite.TriLaser.FireTriLaser += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Dup);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Halcyonite laser crit hook failed!");
                }
            };

            // Halcyonite Whirlwind
            IL.EntityStates.Halcyonite.WhirlWindPersuitCycle.UpdateAttack += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.Before,
                    x => x.MatchStfld<BlastAttack>("crit")) &&
                    c.TryGotoPrev(MoveType.Before,
                    x => x.MatchLdcI4(out _))
                )
                {
                    c.Remove();
                    c.Emit(OpCodes.Ldarg_0);
                    c.EmitDelegate<Func<EntityStates.Halcyonite.WhirlWindPersuitCycle, bool>>((self) =>
                    {
                        return self.characterBody.RollCrit();
                    });

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Halcyonite Whirlwind crit hook failed!");
                }
            };

            // Lynx Totem slam
            On.EntityStates.Totem.GroundPound.OnEnter += (orig, self) =>
            {
                orig(self);

                if (self._attack != null)
                {
                    self._attack.crit = self.RollCrit();
                }
            };

            // Lynx Totem pillar (direct hit)
            IL.EntityStates.Totem.PillarAttack.GetPillarAttack += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Dup);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Lynx Totem pillar crit hook failed!");
                }
            };

            // Xi Construct laser finisher
            IL.EntityStates.MajorConstruct.Weapon.TerminateLaser.OnEnter += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Dup);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Xi Construct laser finisher crit hook failed!");
                }
            };

            // Solus Amalgamator shock pulse
            IL.EntityStates.SolusAmalgamator.ShockArmor.ApplyShock += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Dup);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Solus Amalgamator shock pulse crit hook failed!");
                }
            };

            // Bonus: fix Solus Amalgamator shock pulse not having its attacker properly set
            IL.EntityStates.SolusAmalgamator.ShockArmor.ApplyShock += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("attacker")) &&
                    c.TryGotoPrev(MoveType.Before,
                    x => x.MatchLdnull())
                )
                {
                    c.Remove();
                    c.Emit(OpCodes.Ldarg_0);
                    c.EmitDelegate<Func<EntityStates.SolusAmalgamator.ShockArmor, GameObject>>((self) =>
                    {
                        return self.gameObject;
                    });

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Solus Amalgamator shock pulse attacker hook failed!");
                }
            };

            // Solus Amalgamator energy orb
            IL.EntityStates.SolusAmalgamator.ArtilleryStrike.FireMissiles += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<RoR2.Projectile.FireProjectileInfo>("damage"))
                )
                {
                    c.Emit(OpCodes.Ldloca, 3);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<RoR2.Projectile.FireProjectileInfo>(OpCodes.Stfld, nameof(RoR2.Projectile.FireProjectileInfo.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Solus Amalgamator energy orb crit hook failed!");
                }
            };

            // Severed Breaker
            IL.EntityStates.SolusAmalgamator.MissilePod.OnEnter += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<RoR2.Projectile.FireProjectileInfo>("damage"))
                )
                {
                    c.Emit(OpCodes.Ldloca, 2);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<RoR2.Projectile.FireProjectileInfo>(OpCodes.Stfld, nameof(RoR2.Projectile.FireProjectileInfo.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Severed Breaker crit hook failed!");
                }
            };

            // Shruum landing attack
            IL.EntityStates.BigMush.Regrowth.FireLandingBlastAttack += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.BigMush.Regrowth>(OpCodes.Ldfld, nameof(EntityStates.BigMush.Regrowth._landingBlastAttack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Shruum landing attack crit hook failed!");
                }
            };

            // Mithrix hammer slam (direct)
            IL.EntityStates.BrotherMonster.WeaponSlam.OnEnter += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<OverlapAttack>("damage"))
                )
                {
                    c.Emit(OpCodes.Dup);
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<OverlapAttack>(OpCodes.Stfld, nameof(OverlapAttack.isCrit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Mithrix hammer slam (direct) crit hook failed!");
                }
            };

            // Mithrix hammer slam (explosion)
            IL.EntityStates.BrotherMonster.WeaponSlam.FixedUpdate += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<BlastAttack>("baseDamage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<EntityStates.BrotherMonster.WeaponSlam>(OpCodes.Ldfld, nameof(EntityStates.BrotherMonster.WeaponSlam.blastAttack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<BaseState>(OpCodes.Call, nameof(BaseState.RollCrit));
                    c.Emit<BlastAttack>(OpCodes.Stfld, nameof(BlastAttack.crit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Mithrix hammer slam (explosion) crit hook failed!");
                }
            };

            // Generic contact damage (includes Magma Worm, Overloading Worm & Shruum)
            IL.RoR2.ContactDamage.FireOverlaps += (il) =>
            {
                ILCursor c = new(il);
                bool hookFailed = true;

                if (
                    c.TryGotoNext(MoveType.After,
                    x => x.MatchStfld<OverlapAttack>("damage"))
                )
                {
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<ContactDamage>(OpCodes.Ldfld, nameof(ContactDamage.overlapAttack));
                    c.Emit(OpCodes.Ldarg_0);
                    c.Emit<ContactDamage>(OpCodes.Ldfld, nameof(ContactDamage.characterBody));
                    c.Emit<CharacterBody>(OpCodes.Callvirt, nameof(CharacterBody.RollCrit));
                    c.Emit<OverlapAttack>(OpCodes.Stfld, nameof(OverlapAttack.isCrit));

                    hookFailed = false;
                }

                if (hookFailed == true)
                {
                    Log.Error("Contact damage crit hook failed!");
                }
            };
        }

        private void EnemyCritChance(CharacterBody body, RecalculateStatsAPI.StatHookEventArgs args)
        {
            if (body && body.teamComponent && body.teamComponent.teamIndex != TeamIndex.Player)
            {
                args.critAdd += enemyCritChance.Value;
            }
        }
    }
}
