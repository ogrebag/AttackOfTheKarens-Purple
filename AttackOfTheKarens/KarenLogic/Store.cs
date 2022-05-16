namespace KarenLogic {
  public class Store {
    private Karen karen;
    private SuperKaren superkaren;
    private bool containsOwner;


    public Store(Karen karen) {
      this.karen = karen;
    }

    public Store(SuperKaren superkaren){
       this.superkaren = superkaren;
        }

    public void ActivateTheKaren() {
      karen.Appear();
      superkaren.Appear();
    }

    public void OwnerWalksIn() {
      containsOwner = true;
    }

    public void ResetOwner() {
      containsOwner = false;
    }

    public void Update() {
            if (karen.IsPresent && containsOwner)
            {
                karen.Damage(1);
            }
            else if (superkaren.IsPresent && containsOwner)
            {
                superkaren.Damage(1);
            }
       
      }
    }
  }
