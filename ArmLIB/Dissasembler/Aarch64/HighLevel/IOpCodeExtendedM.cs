namespace ArmLIB.Dissasembler.Aarch64.HighLevel
{
    public interface IOpCodeExtendedM : IOpCodeRm
    {
        public Extend Option    { get; set; }
        public int Shift        { get; set; }
    }
}
