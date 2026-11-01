namespace Tjugoett;

public class Dealer : Participant
{
    private const int StandValue = 17;

    public Dealer() : base("Givaren")
    {
    }

    // Givarens fasta regel: dra kort så länge handen är värd mindre än 17.
    public bool MustDraw => Hand.Value < StandValue;
}