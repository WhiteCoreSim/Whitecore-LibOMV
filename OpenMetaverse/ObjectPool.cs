
/*
 * Copyright (c) 2006-2016, openmetaverse.co
 * All rights reserved.
 *
 * - Redistribution and use in source and binary forms, with or without
 *   modification, are permitted provided that the following conditions are met:
 *
 * - Redistributions of source code must retain the above copyright notice, this
 *   list of conditions and the following disclaimer.
 * - Neither the name of the openmetaverse.co nor the names
 *   of its contributors may be used to endorse or promote products derived from
 *   this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Net;

namespace OpenMetaverse
{
    // this class encapsulates a single packet that
    // is either sent or received by a UDP socket
    public class UDPPacketBuffer
    {
        /// <summary>Size of the byte array used to store raw packet data</summary>
        public const int DEFAULT_BUFFER_SIZE = 4096;
        /// <summary>Raw packet data buffer</summary>
        public readonly byte [] Data;
        /// <summary>Length of the data to transmit</summary>
        public int DataLength;
        /// <summary>EndPoint of the remote host</summary>
        public EndPoint RemoteEndPoint;
        /// <summary>
        /// Was the buffer leased from a pool?
        /// </summary>
        public bool BytesLeasedFromPool;

        /// <summary>
        /// Create an allocated UDP packet buffer for receiving a packet
        /// </summary>
        public UDPPacketBuffer ()
        {
            Data = new byte [DEFAULT_BUFFER_SIZE];
            // Will be modified later by BeginReceiveFrom()
            RemoteEndPoint = new IPEndPoint (Settings.BIND_ADDR, 0);
        }

        /// <summary>
        /// Create an allocated UDP packet buffer for sending a packet
        /// </summary>
        /// <param name="endPoint">EndPoint of the remote host</param>
        public UDPPacketBuffer (IPEndPoint endPoint)
        {
            Data = new byte [DEFAULT_BUFFER_SIZE];
            RemoteEndPoint = endPoint;
        }

        /// <summary>
        /// Create an allocated UDP packet buffer for sending a packet
        /// </summary>
        /// <param name="endPoint">EndPoint of the remote host</param>
        /// <param name="bufferSize">Size of the buffer to allocate for packet data</param>
        public UDPPacketBuffer (IPEndPoint endPoint, int bufferSize)
        {
            Data = new byte [bufferSize];
            RemoteEndPoint = endPoint;
        }

        /// <summary>
        /// Create an allocated UDP packet buffer for sending a packet
        /// </summary>
        public UDPPacketBuffer(byte[] buffer, int bufferSize, IPEndPoint destination, int category, bool fromBufferPool)
        {
            Data = new byte[bufferSize];
            CopyFrom(buffer, bufferSize);
            DataLength = bufferSize;

            RemoteEndPoint = destination;
            BytesLeasedFromPool = fromBufferPool;
        }

        /// <summary>
        /// Create an allocated UDP packet buffer for sending a packet
        /// </summary>
        /// <param name="endPoint">EndPoint of the remote host</param>
        /// <param name="data">The actual buffer to use for packet data (no allocation).</param>
        public UDPPacketBuffer(IPEndPoint endPoint, byte[] data)
        {
            Data = data;
            RemoteEndPoint = endPoint;
        }

        public void CopyFrom(Array src, int length)
        {
            Buffer.BlockCopy(src, 0, Data, 0, length);
        }

        public void CopyFrom(Array src)
        {
            CopyFrom(src, src.Length);
        }

        public void ResetEndpoint()
        {
            RemoteEndPoint = new IPEndPoint(Settings.BIND_ADDR, 0);
        }
    }

    /// <summary>
    /// Object pool for packet buffers. This is used to allocate memory for all
    /// incoming and outgoing packets, and zerocoding buffers for those packets
    /// </summary>
    public class PacketBufferPool : ObjectPoolBase<UDPPacketBuffer>
    {
        readonly IPEndPoint EndPoint;

        /// <summary>
        /// Initialize the object pool in client mode
        /// </summary>
        /// <param name="endPoint">Server to connect to</param>
        /// <param name="itemsPerSegment"></param>
        /// <param name="minSegments"></param>
        public PacketBufferPool (IPEndPoint endPoint, int itemsPerSegment, int minSegments)
        {
            EndPoint = endPoint;
            Initialize (itemsPerSegment, minSegments, true, 1000 * 60 * 5);
        }

        /// <summary>
        /// Initialize the object pool in server mode
        /// </summary>
        /// <param name="itemsPerSegment"></param>
        /// <param name="minSegments"></param>
        public PacketBufferPool (int itemsPerSegment, int minSegments)
        {
            EndPoint = null;
            Initialize (itemsPerSegment, minSegments, true, 1000 * 60 * 5);
        }

        /// <summary>
        /// Returns a packet buffer with EndPoint set if the buffer is in
        /// client mode, or with EndPoint set to null in server mode
        /// </summary>
        /// <returns>Initialized UDPPacketBuffer object</returns>
        protected override UDPPacketBuffer GetObjectInstance ()
        {
            if (EndPoint != null)
                // Client mode
                return new UDPPacketBuffer (EndPoint);
            
            // Server mode
            return new UDPPacketBuffer ();
        }
    }

    public static class Pool
    {
        public static PacketBufferPool PoolInstance;

        /// <summary>
        /// Default constructor
        /// </summary>
        static Pool ()
        {
            PoolInstance = new PacketBufferPool (new IPEndPoint (Settings.BIND_ADDR, 0), 16, 1);
        }

        /// <summary>
        /// Check a packet buffer out of the pool
        /// </summary>
        /// <returns>A packet buffer object</returns>
        public static WrappedObject<UDPPacketBuffer> CheckOut ()
        {
            return PoolInstance.CheckOut ();
        }
    }
}
