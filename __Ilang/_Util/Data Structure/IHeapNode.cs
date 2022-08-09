namespace Ilang
{
    // 힙 구조 노드
    //TODO: Optimize
    public interface IHeapNode {
        int index { get; set; }
        float cost { get; set; }
        void Reset();
    }
}
