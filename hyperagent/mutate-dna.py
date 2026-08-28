import re
import sys
import os

def mutate_system_instructions(original_file="GEMINI.md", proposed_file="GEMINI_NEXT.md"):
    print("[MUTATION] Analyzing proposed Hyperagent rewrite...")
    
    # 1. Read both states
    try:
        with open(original_file, 'r', encoding='utf-8') as f:
            original_dna = f.read()
        with open(proposed_file, 'r', encoding='utf-8') as f:
            proposed_dna = f.read()
    except FileNotFoundError as e:
        print(f"[FATAL ERROR] Missing state files: {e}")
        sys.exit(1)

    # 2. Define the strict boundaries for Section 4 (the MUTABLE section)
    # This regex captures everything between the Section 4 header and the Section 5 header.
    pattern = r"(## 4\. Current Optimization Strategy \(MUTABLE\))(.*?)(## 5\. The Evolutionary Loop & Novelty Constraint)"
    
    # 3. Extract the new logic from the Hyperagent's output
    proposed_match = re.search(pattern, proposed_dna, re.DOTALL)
    if not proposed_match:
        print("[REJECTED] Hyperagent failed to output valid Section 4 structure. Aborting mutation to preserve system integrity.")
        # Clean up the proposed file even on failure if it exists
        if os.path.exists(proposed_file):
            os.remove(proposed_file)
        sys.exit(0)
        
    new_section_4_content = proposed_match.group(2)

    # 4. Splice the new logic into the original DNA
    def splice_replacement(match):
        return f"{match.group(1)}{new_section_4_content}{match.group(3)}"

    mutated_dna, substitution_count = re.subn(pattern, splice_replacement, original_dna, flags=re.DOTALL)

    if substitution_count == 0:
         print("[REJECTED] Could not locate Section 4 in the original GEMINI.md. Structural corruption detected.")
         sys.exit(1)

    # 5. Commit the Mutation
    with open(original_file, 'w', encoding='utf-8') as f:
        f.write(mutated_dna)
        
    # Clean up the temporary proposed file
    if os.path.exists(proposed_file):
        os.remove(proposed_file)
    print("[SUCCESS] DNA Mutated successfully. New instructions locked for next epoch.")

if __name__ == "__main__":
    mutate_system_instructions()
