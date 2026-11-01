
using LAB_2.Decorators;
using LAB_2.Interfaces;
using LAB_2.Model.Characters;

public class MoveForwardCommand : ICommand
{
    private readonly IWarUnit _targetEnemy;
    private readonly IEnumerable<IWarUnit> _allUnits;

    public string Name => "Вперед (Наступ)";

    public MoveForwardCommand(IWarUnit targetEnemy, IEnumerable<IWarUnit> allUnits)
    {
        _targetEnemy = targetEnemy;
        _allUnits = allUnits;
    }

    public void Execute(IWarUnit unit, int mapWidth, int mapHeight)
    {
        if (!unit.IsAlive || _targetEnemy == null || !_targetEnemy.IsAlive) return;

        int dx = Math.Sign(_targetEnemy.X - unit.X);
        int dy = Math.Sign(_targetEnemy.Y - unit.Y);

        var baseChar = UnwrapToBaseCharacter(unit);
        int moveDist = baseChar?.Movement?.MoveDistance ?? 1;
        unit.MoveBy(dx * moveDist, dy * moveDist, mapWidth, mapHeight, _allUnits);
        Console.WriteLine($"[{unit.MapSymbol}] {unit.Name} наступає на [{_targetEnemy.MapSymbol}] {_targetEnemy.Name} -> нова позиція ({unit.X}, {unit.Y})");
    }

    private static Character? UnwrapToBaseCharacter(IWarUnit unit)
    {
        while (unit is CharacterDecorator decorator) unit = decorator.InnerUnit;
        return unit as Character;
    }
}