﻿namespace WootzJs.Testing
{
    public static class Assert
    {
        public static void AssertEquals(this object actual, object expected)
        {
            bool equal = actual == expected;
/*
            if (actual == null && expected != null || actual != null && expected == null)
                equal = false;
            else if (actual == null)
                equal = true;
            else if (actual is bool && expected is bool)
                equal = (bool)actual == (bool)expected;
            else
                equal = actual.Equals(expected);
*/
                
            if (!equal)
                throw new AssertionException("Expected: " + expected + ", Found: " + actual);
        }

        public static void AssertTrue(this bool actual)
        {
            if (!actual)
                throw new AssertionException("Expected true");
        }
    }
}