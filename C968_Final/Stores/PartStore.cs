using C968_Final.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C968_Final.Stores
{
    public class PartStore
    {
        public PartStore()
        {
            m_partId = 0;

            m_partByPartId = new Dictionary<int, PartBase>();
        }

        public void AddPart(PartBase newPart)
        {
            newPart.Id = m_partId;
            m_partByPartId.Add(m_partId, newPart);
            ++m_partId;
        }

        public void UpdatePart(int partId, PartBase newPart)
        {
            m_partByPartId[partId] = newPart;
        }

        public void DeletePart(int partId)
        {
            m_partByPartId.Remove(partId);
        }

        public IReadOnlyList<PartBase> GetParts() => m_partByPartId.Values.ToList();
        public IReadOnlyList<PartBase> GetParts(List<int> ids)
        {
            var parts = new List<PartBase>();
            if (ids is null)
                return parts;

            parts.AddRange(m_partByPartId.Values.Where(part => ids.Contains(part.Id.Value)));
            return parts;
        }

        Dictionary<int, PartBase> m_partByPartId;
        int m_partId;
    }
}
