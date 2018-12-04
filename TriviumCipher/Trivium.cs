using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace TriviumCipher
{
    class Trivium
    {
        private BitArray reg1;
        private BitArray reg2;
        private BitArray reg3;
        private BitArray key;
        private BitArray initVector;

        private const int KEY_LENGTH = 80;

        public Trivium(BitArray key, BitArray initVector)
        {
            if (key.Length != KEY_LENGTH)
            {
                throw new Exception("key length must be " + KEY_LENGTH + " bit");
            }
            if (initVector.Length != KEY_LENGTH)
            {
                throw new Exception("initVector length must be "+ KEY_LENGTH + " bit");
            }
            this.key = key;
            this.initVector = initVector;
            reg1 = new BitArray(93);
            reg2 = new BitArray(84);
            reg3 = new BitArray(111);
        }

        public void Init()
        {
            for (int i = 0; i < reg1.Length; ++i)
            {
                reg1[i] = i < key.Length ? key[i] : false;
            }
            for (int i = 0; i < reg2.Length; ++i)
            {
                reg2[i] = i < initVector.Length ? initVector[i] : false;
            }
            for (int i = 0; i < reg3.Length; ++i)
            {
                reg3[i] = i > 107;
            }

            for (int i = 0; i < 4 * 288; ++i)
            {
                bool t1 = reg1[65] ^ reg1[90] & reg1[91] ^ reg1[92] ^ reg2[77];
                bool t2 = reg2[68] ^ reg2[81] & reg2[82] ^ reg2[83] ^ reg3[86];
                bool t3 = reg3[65] ^ reg3[108] & reg3[109] ^ reg3[110] ^ reg1[68];

                reg1 = ArrayRightShift(reg1, t3);
                reg2 = ArrayRightShift(reg2, t1);
                reg3 = ArrayRightShift(reg3, t2);
            }
        }

        private BitArray GenerateKeyStream(int length)
        {
            BitArray res = new BitArray(length);
            for (int i = 0; i < length; ++i)
            {
                bool t1 = reg1[65] ^ reg1[92];
                bool t2 = reg2[68] ^ reg2[83];
                bool t3 = reg3[65] ^ reg3[110];

                res[i] = t1 ^ t2 ^ t3;

                t1 = t1 ^ reg1[90] & reg1[91] ^ reg2[77];
                t2 = t2 ^ reg2[81] & reg2[82] ^ reg3[86];
                t3 = t3 ^ reg3[108] & reg3[109] ^ reg1[68];

                reg1 = ArrayRightShift(reg1, t3);
                reg2 = ArrayRightShift(reg2, t1);
                reg3 = ArrayRightShift(reg3, t2);
            }

            return res;
        }

        public BitArray Encrypt(BitArray source)
        {
            Init();
            return source.Xor(GenerateKeyStream(source.Length));
        }

        public BitArray Decrypt(BitArray source)
        {
            return Encrypt(source);
        }

        private BitArray ArrayRightShift(BitArray array, bool val)
        {
            BitArray res = new BitArray(array.Length)
            {
                [0] = val
            };
            for (int i = 1; i < array.Length; ++i)
            {
                res[i] = array[i - 1];
            }
            return res;
        }
    }
}
