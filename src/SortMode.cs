namespace QM_CloneSort
{
    public enum SortMode
    {
        /// <summary>User-defined order, maintained across sessions. Rows show an up-arrow icon.</summary>
        Manual = 0,
        /// <summary>Alphabetical by display name, A -> Z.</summary>
        NameAsc = 1,
        /// <summary>Alphabetical by display name, Z -> A.</summary>
        NameDesc = 2,
        /// <summary>Highest rank first, then A -> Z within the same rank.</summary>
        RankDesc = 3,
        /// <summary>Alphabetical by class name, A -> Z, then A -> Z by display name within the same class.</summary>
        ClassAsc = 4,
        /// <summary>Alphabetical by class name, Z -> A, then A -> Z by display name within the same class.</summary>
        ClassDesc = 5
    }
}
