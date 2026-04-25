namespace SamEngine
{
    public struct ButtonState
    {
        public bool Held;

        public bool Clicked;

        public bool Released;

        public void Update(bool heldThisFrame)
        {
            this.Clicked = heldThisFrame && !this.Held;
            this.Released = !heldThisFrame && this.Held;
            this.Held = heldThisFrame;
        }

    }
}
