public interface IDamageReciever
{
    int Health { get; }
    void Damage(int damage);
}