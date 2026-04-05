// ItemEnums.cs — 物品系统枚举定义

public enum ItemType
{
    Survival,       // 生存刚需（大米、罐头、抗生素、五金工具）
    Collectible     // 时代收集品（90年代老物件）
}

public enum ItemRarity
{
    Common,         // 普通
    Uncommon,       // 罕见
    Rare,           // 稀有
    Epic            // 史诗
}

public enum SurvivalEffectType
{
    None,
    RestoreHealth,      // 恢复生命值
    RestoreHunger,      // 恢复饱腹度
    CureInfection,      // 治疗感染（抗生素）
    RepairEquipment     // 修复耐久（五金工具）
}
