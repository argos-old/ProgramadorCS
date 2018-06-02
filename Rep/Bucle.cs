using NAudio.Wave;

namespace Rep
{ 
    // AUTOR: Mark Heath
    // http://mark-dot-net.blogspot.com.es/2009/10/looped-playback-in-net-with-naudio.html

    /// <summary>
    /// Stream for looping playback
    /// </summary>
    public class LoopStream : WaveStream
    {
        WaveStream sourceStream;

        /// <summary>
        /// Creates a new Loop stream
        /// </summary>
        /// <param name="sourceStream">The stream to read from. Note: the Read method of this stream should return 0 when 
        /// it reaches the end or else we will not loop to the start again.</param>
        /// <param name="enableLooping">Indica si se habilitan los bucles o no. NOTA: Añadido en el constructor para que pueda
        /// utilizarse en todas las reproducciones junto con la bool reproduccionBucle de Avisos de forma simple.</param>
        public LoopStream(WaveStream sourceStream, bool enableLooping = true)
        {
            this.sourceStream = sourceStream;
            this.EnableLooping = enableLooping;
        }

        /// <summary>
        /// Use this to turn looping on or off
        /// </summary>
        public bool EnableLooping
        {
            get;
            set;
        }

        /// <summary>
        /// Return source stream's wave format
        /// </summary>
        public override WaveFormat WaveFormat
        {
            get
            {
                return sourceStream.WaveFormat;
            }
        }

        /// <summary>
        /// LoopStream simply returns
        /// </summary>
        public override long Length
        {
            get
            {
                return sourceStream.Length;
            }
        }

        /// <summary>
        /// LoopStream simply passes on positioning to source stream
        /// </summary>
        public override long Position
        {
            get
            {
                return sourceStream.Position;
            }
            set
            {
                sourceStream.Position = value;
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int totalBytesRead = 0;

            while (totalBytesRead < count)
            {
                int bytesRead = sourceStream.Read(buffer, offset + totalBytesRead, count - totalBytesRead);

                if (bytesRead == 0)
                {
                    if (sourceStream.Position == 0 || !EnableLooping)
                    {
                        // something wrong with the source stream
                        //CHAPUZA PARA INDICAR EL FINAL DEL STREAM:
                        //new Reproductor().StreamFinalizado = false;
                        break;
                    }

                    // loop
                    sourceStream.Position = 0;
                }

                totalBytesRead += bytesRead;
            }

            return totalBytesRead;
        }
    }
}
