namespace KarenLogic {
  public class Store {
    private Karen karen;
    private bool containsOwner;
    private bool containsBomb;
    private bool horde;

    public Store(Karen karen) {
      this.karen = karen;
    }

    public void ActivateTheKaren() {
      karen.Appear();
    }

    public void OwnerWalksIn() {
      containsOwner = true;
    }

    public void BombUsed()
        {
            containsBomb = true;
        }

    public void BombFinished()
        {
            containsBomb = false;
        }

    public void HordeStart()
        {
            horde = true;
        }

    public void HordeEnd()
        {
            horde = false;
        }

    public void ResetOwner() {
      containsOwner = false;
    }

    public void Update() {
      if (karen.IsPresent && containsOwner) {
        karen.Damage(1);
      }
      if(karen.IsPresent && containsBomb)
            {
            karen.Damage(1);
            }
            if (!karen.IsPresent && horde)
            {
                karen.Appear();
            }
    }
  }
}
