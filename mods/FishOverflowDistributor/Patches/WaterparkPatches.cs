using System;
using System.Collections.Generic;
using System.Linq;
using Harmony;
using SharedCode.Utils;
using UnityEngine;

namespace FishOverflowDistributor.Patches
{
    [HarmonyPatch(typeof(WaterPark))]
    [HarmonyPatch("TryBreed")]
    internal class WaterparkPatches
    {
        // ReSharper disable once InconsistentNaming
        private static bool Prefix(WaterPark __instance, WaterParkCreature creature)
        {
            FishOverflowDistributor.Logger.LogTrace("WaterPark.TryBreed(WaterParkCreature) called.");

            //We only distribute when the WaterPark doesn't have any space left for bred creatures.
            //This is the only place in this method where we allow the original unpatched method to execute, 
            //because the original method is never executed when there is no space left in the WaterPark.

            FishOverflowDistributor.Logger.LogTrace(
                $"__instance.HasFreeSpace() returns '{__instance.HasFreeSpace()}'.");

            if (__instance.HasFreeSpace())
                return true;

            //using Harmony's Traverse class for reflection. Harmony caches MethodInfo, FieldInfo, etc. for further use and thus increases performance.
            var items = Traverse
                        .Create(__instance)
                        .Field("items")
                        .GetValue<List<WaterParkItem>>();

            if (items == null)
            {
                FishOverflowDistributor.Logger.LogError(
                    "FieldInfo or value for field 'items' in class 'WaterParkItem' with type 'List<WaterParkItem>' was null -> Should not happen, investigate!.");

                return false;
            }


            FishOverflowDistributor.Logger.LogTrace("Checking if creature is contained in items");

            //Don't know why this check is needed. Maybe TryBreed gets called on fish which arent contained in this WaterPark instance.
            if (!items.Contains(creature))
                return false;

            TechType creatureTechType = creature.pickupable.GetTechType();
            FishOverflowDistributor.Logger.LogTrace($"Creature Tech Type = {creatureTechType.ToString()}.");

            FishOverflowDistributor.Logger.LogTrace(
                "Checking whether creatureEggs.containsKey(creatureTechType)");
#if SUBNAUTICA
            //we don't want to distribute creature eggs
            if (WaterParkCreature.creatureEggs.ContainsKey(creatureTechType))
                return false;
#endif
            FishOverflowDistributor.Logger.LogTrace(
                $"Waterpark '{__instance.gameObject.name}' contains creature '{creature.gameObject.name}' and has enough space for another one.");

            var secondCreature = items.Find(item =>
                                                item != creature &&
                                                item is WaterParkCreature &&

                                                // ReSharper disable once TryCastAlwaysSucceeds
                                                (item as WaterParkCreature).GetCanBreed() &&
                                                item.pickupable != null &&
                                                item.pickupable.GetTechType() == creatureTechType) as
                WaterParkCreature;

            if (secondCreature == null)
                return false;

            FishOverflowDistributor.Logger.LogTrace(
                $"Waterpark contains two creatures '{creature.gameObject.name}' of TechType '{creatureTechType.ToString()}' which can breed with each other.");

            BaseBioReactor suitableReactor;
            try
            {
                //Get a reactor which has space for the item in the same base
                suitableReactor = SubnauticaSceneTraversalUtils
                                                 .GetComponentsInSameBase<BaseBioReactor>(__instance.gameObject)
                                                 .First(
                                                     reactor =>
                                                     {
                                                         var itemsContainer = Traverse
                                                                              .Create(reactor)
                                                                              .Property("container")
                                                                              .GetValue<ItemsContainer>();

                                                         if (itemsContainer != null)
                                                             return itemsContainer.HasRoomFor(
                                                                 creature.pickupable);

                                                         FishOverflowDistributor.Logger.LogTrace(
                                                             $"PropertyInfo or value for property 'container' in class 'BaseBioReactor' with type 'ItemsContainer' was null -> Should not happen, investigate!.");

                                                         return false;
                                                     });
            }
            catch (Exception)
            {
                return false;
            }
            if (suitableReactor == null)
            {
                FishOverflowDistributor.Logger.LogTrace("Could not find suitable reactor");
                return false;
            }

            //Reset breed time of the second creature so it can't be used to immediately breed again.
            secondCreature.ResetBreedTime();

            FishOverflowDistributor.Logger.LogTrace(
                $"Found suitable reactor '{suitableReactor.gameObject.name}'.");

            //Now we create a pickupable from the WaterParkCreature which we can add to the reactor's inventory.
            //Because the creature can't be taken out from the reactor inventory, we don't need to add WaterparkCreature component
            //to it. This would be needed so the game knows when you drop it outside, that it came from a waterpark.


            GameObject newCreature = CraftData.InstantiateFromPrefab(creatureTechType, false);
            newCreature.SetActive(false);
            newCreature.transform.position = creature.transform.position + Vector3.down;
            var pickupable = newCreature.EnsureComponent<Pickupable>();

            /*WaterParkCreatureParameters creatureParameters =
                WaterParkCreature.GetParameters(creatureTechType);

            newCreature.transform.localScale = creatureParameters.initialSize * Vector3.one;
            var newCreatureComponent = newCreature.AddComponent<WaterParkCreature>();
            newCreatureComponent.age = 0f;

            Traverse
                .Create(newCreatureComponent)
                .Field("parameters")
                .SetValue(creatureParameters);

            Pickupable pickupable = creatureParameters.isPickupableOutside
                ? newCreature.EnsureComponent<Pickupable>()
                : newCreature.GetComponent<Pickupable>();

            newCreature.setActive();*/
#if SUBNAUTICA
            pickupable = pickupable.Pickup(false);
#elif BELOWZERO
            pickupable.Pickup(false);
#endif
            // pickupable.GetComponent<WaterParkItem>()?.SetWaterPark(null);

            var itemToAdd = new InventoryItem(pickupable);

            var reactorItemsContainer = Traverse
                                        .Create(suitableReactor)
                                        .Property("container")
                                        .GetValue<ItemsContainer>();

            if (reactorItemsContainer == null)
            {
                FishOverflowDistributor.Logger.LogError(
                    $"PropertyInfo or value for property 'container' in class 'BaseBioReactor' with type 'ItemsContainer' was null -> Should not happen, investigate!.");

                return false;
            }

            reactorItemsContainer.AddItem(pickupable);

            return false;
        }
    }
}
