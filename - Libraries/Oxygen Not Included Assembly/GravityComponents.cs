// Decompiled with JetBrains decompiler
// Type: GravityComponents
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: BA533216-CB4F-43C8-8FF5-02CE00126C4B
// Assembly location: C:\Games\steamapps\common\OxygenNotIncluded\OxygenNotIncluded_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

public class GravityComponents : KGameObjectComponentManager<GravityComponent>
{
  private static Tag[] LANDS_ON_FAKEFLOOR = new Tag[3]
  {
    GameTags.Minion,
    GameTags.Creatures.Walker,
    GameTags.Creatures.Hoverer
  };
  private const float Acceleration = -9.8f;

  public HandleVector<int>.Handle Add(
    GameObject go,
    Vector2 initial_velocity,
    System.Action on_landed = null)
  {
    bool land_on_fake_floors = false;
    KPrefabID component = go.GetComponent<KPrefabID>();
    if ((UnityEngine.Object) component != (UnityEngine.Object) null)
      land_on_fake_floors = component.HasAnyTags(GravityComponents.LANDS_ON_FAKEFLOOR);
    return this.Add(go, new GravityComponent(go.transform, on_landed, initial_velocity, land_on_fake_floors));
  }

  public override void FixedUpdate(float dt)
  {
    GravityComponents.Tuning tuning = TuningData<GravityComponents.Tuning>.Get();
    float num1 = tuning.maxVelocity * tuning.maxVelocity;
    for (int index = 0; index < this.data.Count; ++index)
    {
      GravityComponent gravityComponent = this.data[index];
      if ((double) gravityComponent.elapsedTime >= 0.0 && !((UnityEngine.Object) gravityComponent.transform == (UnityEngine.Object) null))
      {
        Vector3 position = gravityComponent.transform.GetPosition();
        Vector2 pos1 = (Vector2) position;
        Vector2 vector2_1 = new Vector2(gravityComponent.velocity.x, gravityComponent.velocity.y + -9.8f * dt);
        float sqrMagnitude = vector2_1.sqrMagnitude;
        if ((double) sqrMagnitude > (double) num1)
          vector2_1 *= tuning.maxVelocity / Mathf.Sqrt(sqrMagnitude);
        int cell1 = Grid.PosToCell(pos1);
        bool flag1 = Grid.IsVisiblyInLiquid(pos1 + new Vector2(0.0f, gravityComponent.radius));
        if (flag1)
        {
          flag1 = true;
          float b = (float) (gravityComponent.transform.GetInstanceID() % 1000) / 1000f * 0.25f;
          float num2 = tuning.maxVelocityInLiquid + b * tuning.maxVelocityInLiquid;
          if ((double) sqrMagnitude > (double) num2 * (double) num2)
          {
            float a = Mathf.Sqrt(sqrMagnitude);
            vector2_1 = vector2_1 / a * Mathf.Lerp(a, b, dt * (float) (5.0 + 5.0 * (double) b));
          }
        }
        gravityComponent.velocity = vector2_1;
        gravityComponent.elapsedTime += dt;
        Vector2 vector2_2 = pos1 + vector2_1 * dt;
        Vector2 pos2 = vector2_2;
        pos2.y -= gravityComponent.radius;
        bool flag2 = Grid.IsVisiblyInLiquid(vector2_2 + new Vector2(0.0f, gravityComponent.radius));
        if (!flag1 && flag2)
        {
          KBatchedAnimController effect = FXHelpers.CreateEffect("splash_step_kanim", new Vector3(vector2_2.x, vector2_2.y, 0.0f) + new Vector3(-0.38f, 0.75f, -0.1f), (Transform) null, false, Grid.SceneLayer.FXFront, false);
          effect.Play((HashedString) "fx1", KAnim.PlayMode.Once, 1f, 0.0f);
          effect.destroyOnAnimComplete = true;
        }
        int cell2 = Grid.PosToCell(pos2);
        if (Grid.IsValidCell(cell2))
        {
          if ((double) vector2_1.sqrMagnitude > 0.200000002980232 && Grid.IsValidCell(cell1) && (!Grid.Element[cell1].IsLiquid && Grid.Element[cell2].IsLiquid))
          {
            AmbienceType ambience = Grid.Element[cell2].substance.GetAmbience();
            if (ambience != AmbienceType.None)
            {
              string str = Sounds.Instance.OreSplashSoundsMigrated[(int) ambience];
              if ((UnityEngine.Object) CameraController.Instance != (UnityEngine.Object) null && CameraController.Instance.IsAudibleSound((Vector3) vector2_2, str))
                SoundEvent.PlayOneShot(str, (Vector3) vector2_2);
            }
          }
          bool flag3 = Grid.Solid[cell2];
          if (!flag3 && gravityComponent.landOnFakeFloors && Grid.FakeFloor[cell2])
          {
            Navigator component = gravityComponent.transform.GetComponent<Navigator>();
            if ((bool) ((UnityEngine.Object) component))
            {
              flag3 = component.NavGrid.NavTable.IsValid(cell2, NavType.Floor);
              if (!flag3)
              {
                int cell3 = Grid.CellAbove(cell2);
                flag3 = component.NavGrid.NavTable.IsValid(cell3, NavType.Hover);
              }
            }
          }
          if (flag3)
          {
            Vector3 posCbc = Grid.CellToPosCBC(Grid.CellAbove(cell2), Grid.SceneLayer.Move);
            vector2_2.y = posCbc.y + gravityComponent.radius;
            gravityComponent.velocity.x = 0.0f;
            gravityComponent.elapsedTime = -1f;
            gravityComponent.transform.SetPosition(new Vector3(vector2_2.x, vector2_2.y, position.z));
            this.data[index] = gravityComponent;
            gravityComponent.transform.gameObject.Trigger(1188683690, (object) vector2_1);
            if (gravityComponent.onLanded != null)
              gravityComponent.onLanded();
          }
          else
          {
            Vector2 pos3 = vector2_2;
            pos3.x -= gravityComponent.radius;
            int cell3 = Grid.PosToCell(pos3);
            if (Grid.IsValidCell(cell3) && Grid.Solid[cell3])
            {
              vector2_2.x = Mathf.Floor(vector2_2.x - gravityComponent.radius) + (1f + gravityComponent.radius);
              gravityComponent.velocity.x = -0.1f * gravityComponent.velocity.x;
              this.data[index] = gravityComponent;
            }
            else
            {
              Vector3 pos4 = (Vector3) vector2_2;
              pos4.x += gravityComponent.radius;
              int cell4 = Grid.PosToCell(pos4);
              if (Grid.IsValidCell(cell4) && Grid.Solid[cell4])
              {
                vector2_2.x = Mathf.Floor(vector2_2.x + gravityComponent.radius) - gravityComponent.radius;
                gravityComponent.velocity.x = -0.1f * gravityComponent.velocity.x;
                this.data[index] = gravityComponent;
              }
            }
            gravityComponent.transform.SetPosition(new Vector3(vector2_2.x, vector2_2.y, position.z));
            this.data[index] = gravityComponent;
          }
        }
        else
        {
          gravityComponent.transform.SetPosition(new Vector3(vector2_2.x, vector2_2.y, position.z));
          this.data[index] = gravityComponent;
        }
      }
    }
  }

  public class Tuning : TuningData<GravityComponents.Tuning>
  {
    public float maxVelocity;
    public float maxVelocityInLiquid;
  }
}
