using Godot;

public partial class CatAttackAttack : CharacterBody2D
{
    [Export] Timer timeUntilDestroyTimer;
    [Export] Area2D area;
    [Export] GpuParticles2D dazedCatParticles;
    Vector2 velocity;
    Vector2 direction;
    [Export] AudioStreamPlayer attackHitSFX;

    public override void _Ready()
    {
        timeUntilDestroyTimer.Timeout += OnTimeUntilDestroyTimeout;
        area.AreaEntered += OnAreaEntered;
    }

    public override void _PhysicsProcess(double delta)
    {
        velocity = direction * 1000f;
        Velocity = velocity;
        MoveAndSlide();
    }

    private void OnTimeUntilDestroyTimeout()
    {
        QueueFree();
    }

    public void SetDirAndVel(Vector2 mousePos)
    {
        direction = (mousePos - GlobalPosition).Normalized();
    }

    private void OnAreaEntered(Area2D area)
    {
        RemoveChild(attackHitSFX);
        GetParent().AddChild(attackHitSFX);
        attackHitSFX.Play();
        CatBurgler catBurg = (CatBurgler)area.GetParent();
        catBurg.HitByCatAttack();
        RemoveChild(dazedCatParticles);
        GetParent().AddChild(dazedCatParticles);
        dazedCatParticles.GlobalPosition = GlobalPosition;
        dazedCatParticles.Emitting = true;
        QueueFree();
    }
}
