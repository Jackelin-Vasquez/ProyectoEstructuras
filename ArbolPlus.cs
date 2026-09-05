using System.Collections.Generic;

public class ArbolBPlus
{
    private int orden;
    private int maxClaves;
    private NodoBPlus raiz;

    public ArbolBPlus(int orden = 4)
    {
        this.orden = orden;
        this.maxClaves = orden - 1;
        this.raiz = new NodoBPlus(true);
    }
}