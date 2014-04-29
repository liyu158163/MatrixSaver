using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using FerretLib.SFML;
using SFML.Graphics;
using SFML.Window;

namespace MatrixScreen
{
    public class GlyphStreamManager : IEntity
    {
        private const int MAX_STREAMS = 10;
        private const float CHANCE_OF_NEW_STREAM = 0.2f; // TODO - implement chance of occurring per second, max MAX_STREAMS

        private readonly Rectangle _workingArea;

        private List<GlyphStream> streams;

        public GlyphStreamManager(Vector2u workingArea)
        {
            _workingArea = workingArea.ToRectangle();
            streams = new List<GlyphStream>();
        }

        private void AddNewGlyphStream()
        {
            var stream = new GlyphStream();
            stream.movementRate = GetRandom.Float(50, 300);
            stream.numberOfGlyphs = GetRandom.Int(3, 6);
            stream.scale = GetRandom.Float(0.1f, 0.4f);

            stream.GlyphPosition = new Vector2f(
                GetRandom.Int((int)-stream.Size.X, _workingArea.Width + (int)stream.Size.X),
                GetRandom.Int((int)-stream.Size.Y, _workingArea.Width + (int)stream.Size.Y));

            stream.Position = new Vector2f(stream.GlyphPosition.X, stream.GlyphPosition.Y - stream.Size.Y);

            stream.ClipGlyphs(_workingArea);
            streams.Add(stream);
        }

        public void Render(RenderTarget canvas)
        {
            streams.ForEach(x => x.Render(canvas));
        }

        public void Update(ChronoEventArgs chronoArgs)
        {
            streams.ForEach(x => x.Update(chronoArgs.Delta));

            // Cull completed streams
            streams = streams.Where(
                x => !(
                    x.Position.Y > _workingArea.Bottom
                    || x.Position.Y > x.GlyphPosition.Y + x.Size.Y
                )).ToList();

            // Add new streams
            if (streams.Count < MAX_STREAMS && GetRandom.Float(0,1) < CHANCE_OF_NEW_STREAM)
                AddNewGlyphStream();
        }
    }
}