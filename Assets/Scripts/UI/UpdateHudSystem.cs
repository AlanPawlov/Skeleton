using Leopotam.EcsLite;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateHudSystem : IEcsRunSystem 
{
    public void Run(IEcsSystems systems)
    {
        var sharedData = systems.GetShared<ECSSharedData>();
        if (sharedData.IsPause || sharedData.IsPlayerDeath)
        {
            return;
        }
        var world = systems.GetWorld();
        //var hudFilter = world.Filter<HudComponent>().Inc<UpdateHudComponent>().End();// TODO: ����� ���� ������������ �� ���������� hud � ecs
        var hudFilter = world.Filter<HudComponent>().End();                            // ���� ��������� ������ ��� ������� ����
        var healthFilter = world.Filter<Health>().Inc<PlayerTag>().End();              // �� ��������� ��� ������ ��� ��� ��������
        var stackFilter = world.Filter<ItemsStack>().Inc<PlayerTag>().End();           // ������� ���� ���������� ����� �������� � ������ �����
        var hudPool= world.GetPool<HudComponent>();
        var healthPool = world.GetPool<Health>();
        var stackPool = world.GetPool<ItemsStack>();
        var healthBarFill = 0f;
        var maxStackSize = 0;
        var curStackSize = 0;

        foreach (var e in healthFilter)
        {
            var health = healthPool.Get(e);
            healthBarFill = ((float)health.CurHealth / health.MaxHealth);
        }

        foreach (var e in stackFilter)
        {
            var stack = stackPool.Get(e);
            curStackSize = stack.Items.Count;
            maxStackSize = stack.MaxSize;
        }

        foreach (var hudEntity in hudFilter)
        {
            var hud = hudPool.Get(hudEntity);
            hud.HealthBar.fillAmount = healthBarFill;
            hud.StackCounterText.text = $"{curStackSize}/{maxStackSize}";
        }
    }
}
